import { Component, EventEmitter, inject, input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { StoryTaskEditService } from '../../services/story-task-edit.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { TaskDetails } from '../../models/task-details';
import { EditorStateServiceService } from '../../../../shared/services/editor-state-service.service';

@Component({
  selector: 'app-edit-task-details',
  standalone: true,
  imports: [
    CommonModule,
    MaterialModule,
    TranslateModule,
    DescriptionDiffComponent,
    MarkdownEditorComponent,
  ],
  templateUrl: './edit-task-details.component.html',
})
export class EditTaskDetailsComponent {
  private readonly translateService = inject(TranslateService);
  private readonly storyTaskEditService = inject(StoryTaskEditService);
  private readonly toastService = inject(ToastService);
  private readonly editorStateServiceService = inject(
    EditorStateServiceService
  );

  @Output() taskDetailsUpdate: EventEmitter<TaskDetails> = new EventEmitter();

  isEditorVisible = this.editorStateServiceService.isEditorVisible;
  isSuggestionsVisible = this.editorStateServiceService.isSuggestionsVisible;
  descriptionEditorValue =
    this.editorStateServiceService.descriptionEditorValue;
  suggestionString = this.editorStateServiceService.suggestionString;

  details = input.required<TaskDetails | null>();
  taskId = input.required<number>();
  isLoading = input.required<boolean>();

  updateDescription(newValue: string) {
    this.editorStateServiceService.updateDescription(newValue);
  }

  rejectSuggestion() {
    this.editorStateServiceService.suggestionsVisible = false;
  }

  replaceWithSuggestion() {
    if (this.details() == null) {
      return;
    }
    this.editorStateServiceService.replaceWithSuggestion();
  }

  appendSuggestionToBack() {
    if (this.details() == null) {
      return;
    }
    this.editorStateServiceService.appendSuggestionToBack();
  }

  appendSuggestionToFront() {
    if (this.details() == null) {
      return;
    }
    this.editorStateServiceService.appendSuggestionToFront();
  }

  resetDescription() {
    this.editorStateServiceService.resetDescription(
      this.details()?.description ?? ''
    );
  }

  saveDescriptionChanges() {
    if (
      this.details()?.description ===
      this.editorStateServiceService.descriptionEditorValue()
    ) {
      this.toastService.openError(
        this.translateService.instant('Tasks.Edit.NoChanges')
      );
      return;
    }

    this.storyTaskEditService
      .patchTaskDescription(
        this.taskId(),
        this.details()?.version ?? 0,
        this.editorStateServiceService.descriptionEditorValue()
      )
      .subscribe({
        next: (response) => {
          this.taskDetailsUpdate.emit(response);
          this.editorStateServiceService.setDescriptionEditorValue =
            response.description ?? '';
          this.toastService.openSuccess(
            this.translateService.instant('Tasks.UpdatedSuccessfully')
          );
          this.editorStateServiceService.editorVisible = false;
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('Tasks.ErrorWhileUpdate')
          ),
      });
  }
}
