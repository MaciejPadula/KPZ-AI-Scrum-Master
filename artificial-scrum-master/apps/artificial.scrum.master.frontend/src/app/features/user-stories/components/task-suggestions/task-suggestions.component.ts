import { Component, EventEmitter, inject, Output, input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'apps/artificial.scrum.master.frontend/src/app/shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { GenerateTaskSuggestionsResponse, TaskSuggestion } from '../../models/get-task-suggestions-response';
import { ConfirmCreateTaskDialogComponent } from '../confirn-create-task-dialog/confirm-create-task-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { ToastService } from 'apps/artificial.scrum.master.frontend/src/app/shared/services/toast.service';
import { CreateTaskRequest } from '../../models/create-task-request';
import { TaskService } from '../../services/task.service';

@Component({
  selector: 'app-task-suggestions',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './task-suggestions.component.html',
})
export class TaskSuggestionsComponent implements OnInit {
  @Output() closeSuggestionsEvent: EventEmitter<boolean> = new EventEmitter();
  taskSuggestions = input.required<GenerateTaskSuggestionsResponse>();
  taskSuggestionsList: GenerateTaskSuggestionsResponse;

  projectId = input.required<number>();
  storyId = input.required<number>();

  #dialog = inject(MatDialog);
  #toastService = inject(ToastService);
  #translateService = inject(TranslateService);

  private readonly taskService = inject(TaskService);

  ngOnInit() {
    this.taskSuggestionsList = this.taskSuggestions();
  }

  closeSuggestions() {
    this.closeSuggestionsEvent.emit(true);
  }

  acceptSuggestion(suggestion: TaskSuggestion) {
    if (this.taskSuggestionsList == null || this.taskSuggestionsList.tasks.length == 0) {
      return;
    }
    const dialogRef = this.#dialog.open(ConfirmCreateTaskDialogComponent, {
      data: {
        delete: false,
        task: suggestion
      }
    });
    dialogRef.afterClosed().subscribe((result) => {

      if (result === true) {
        suggestion.accepted = true;

        this.taskService.createTask(suggestion.description, suggestion.title, this.projectId(), this.storyId())
          .subscribe({
            next: () => {
              this.removeSuggestionFromList(suggestion);
              this.#toastService.openSuccess(this.#translateService.instant("UserStories.GenerateTask.Success"));
            },
            error: () => {
              this.#toastService.openError(this.#translateService.instant("UserStories.GenerateTask.Failure"));
            }
          });
      }
    });
  }

  removeSuggestion(suggestion: TaskSuggestion) {
    const dialogRef = this.#dialog.open(ConfirmCreateTaskDialogComponent, {
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
    const updatedTasks = this.taskSuggestionsList.tasks.filter((s) => s !== suggestion) ?? this.taskSuggestionsList?.tasks;
    const updatedSuggestions = { tasks: updatedTasks };
    this.taskSuggestionsList = updatedSuggestions;

    if (updatedSuggestions.tasks.length == 0) {
      this.closeSuggestions();
    }
  }
}
