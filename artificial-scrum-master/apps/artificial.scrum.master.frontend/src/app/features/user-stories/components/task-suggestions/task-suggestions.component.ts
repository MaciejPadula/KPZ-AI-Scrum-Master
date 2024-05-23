import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
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
export class TaskSuggestionsComponent {
  @Output() closeSuggestionsEvent: EventEmitter<boolean> = new EventEmitter();
  @Input() taskSuggestions: GenerateTaskSuggestionsResponse;

  @Input() projectId: number;
  @Input() storyId: number;

  #dialog = inject(MatDialog);
  #httpClient = inject(HttpClient);
  #toastService = inject(ToastService);
  #translateService = inject(TranslateService);

  private readonly taskService = inject(TaskService);

  closeSuggestions() {
    this.closeSuggestionsEvent.emit(true);
  }

  acceptSuggestion(suggestion: TaskSuggestion) {
    if (this.taskSuggestions == null || this.taskSuggestions.tasks.length == 0) {
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

        this.taskService.createTask(suggestion.description, suggestion.title, this.projectId, this.storyId)
          .subscribe({
            next: (response) => {
              this.removeSuggestionFromList(suggestion);
              this.#translateService.get("UserStories.GenerateTask.Success").subscribe({
                next: key => this.#toastService.openSuccess(key),
              }
              );
            },
            error: () => {
              this.#translateService.get("UserStories.GenerateTask.Failure").subscribe({
                next: key => this.#toastService.openError(key),
              });
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
    const updatedTasks = this.taskSuggestions.tasks.filter((s) => s !== suggestion) ?? this.taskSuggestions?.tasks;
    const updatedSuggestions = { tasks: updatedTasks };
    this.taskSuggestions = updatedSuggestions
    if (updatedSuggestions.tasks.length == 0) {
      this.closeSuggestions();
    }
  }
}
