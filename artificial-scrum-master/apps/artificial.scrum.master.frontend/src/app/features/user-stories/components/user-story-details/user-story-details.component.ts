import { Component, Inject, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './../../../../shared/material.module';
import { UserStoryDetails } from '../../models/user-story-details';
import { HttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { GenerateTaskSuggestionsResponse, TaskSuggestion } from '../../models/get-task-suggestions-response';
import { CreateTaskRequest } from '../../models/create-task-request';
import { ToastService } from 'apps/artificial.scrum.master.frontend/src/app/shared/services/toast.service';
import { ConfirmCreateTaskDialogComponent } from '../confirn-create-task-dialog/confirm-create-task-dialog.component';

@Component({
  selector: 'app-user-story-details',
  standalone: true,
  templateUrl: './user-story-details.component.html',
  styleUrls: ['./user-story-details.component.scss'],
  imports: [CommonModule, MaterialModule, TranslateModule],
})
export class UserStoryDetailsComponent implements OnInit {
  details = signal<UserStoryDetails | null>(null);
  taskSuggestions = signal<GenerateTaskSuggestionsResponse | null>(null);
  error = signal<boolean>(false);
  #httpClient = inject(HttpClient);
  #storyId: number;
  #projectId: number;
  suggestionsOpen = signal(false);
  private readonly toastService = inject(ToastService);

  private readonly dialog = inject(MatDialog);

  constructor(@Inject(MAT_DIALOG_DATA) data: { userStoryId: number, projectId: number }) {
    this.#storyId = data.userStoryId;
    this.#projectId = data.projectId;
  }

  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/user-story/generate-tasks';

  ngOnInit(): void {
    this.#httpClient
      .get<UserStoryDetails>(`/api/userStories/${this.#storyId}`)
      .subscribe({
        next: (response) => this.details.set(response),
        error: () => this.error.set(true),
      });
  }

  generateTaskSuggestions() {
    if (this.details() == null || !this.details()?.description) {
      return;
    }
    this.suggestionsOpen.set(true);

    this.httpClient.post<GenerateTaskSuggestionsResponse>(`${this.baseApiUrl}`, {
      UserStoryTitle: this.details()!.title,
      UserStoryDescription: this.details()!.description,
    })
      .subscribe({
        next: (response) => this.taskSuggestions.set(response),
        error: () => this.error.set(true),
      });
  }

  acceptSuggestion(suggestion: TaskSuggestion) {
    if (this.taskSuggestions() == null || this.taskSuggestions()?.tasks.length == 0) {
      return;
    }
    const dialogRef = this.dialog.open(ConfirmCreateTaskDialogComponent, {
      data: {
        delete: false,
        task: suggestion
      }
    });
    dialogRef.afterClosed().subscribe((result) => {

      if (result === true) {
        suggestion.accepted = true;

        console.log(`Accepted suggestion: ${suggestion.title}`);

        this.httpClient.post<CreateTaskRequest>("/api/task", {
          Description: suggestion.description,
          Subject: suggestion.title,
          ProjectId: this.#projectId,
          UserStoryId: this.#storyId,
        })
          .subscribe({
            next: (response) => {
              this.removeSuggestionFromList(suggestion);
              this.toastService.openSuccess("Task created successfully");
            },
            error: () => this.toastService.openError("Error creating task"),
          });
      }
    });
  }

  removeSuggestion(suggestion: TaskSuggestion) {
    const dialogRef = this.dialog.open(ConfirmCreateTaskDialogComponent, {
      data: {
        delete: true,
        task: suggestion
      }
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result === true) {
        this.removeSuggestionFromList(suggestion);
      }
    });
  }

  removeSuggestionFromList(suggestion: TaskSuggestion) {
    const updatedTasks = this.taskSuggestions()!.tasks.filter((s) => s !== suggestion) ?? this.taskSuggestions()?.tasks;
    const updatedSuggestions = { tasks: updatedTasks };
    this.taskSuggestions.set(updatedSuggestions);
    if(updatedSuggestions.tasks.length == 0)
    {
      this.closeSuggestions();
    }
  }

  closeSuggestions()
  {
    this.suggestionsOpen.set(false);
    this.taskSuggestions.set(null);
  }
}
