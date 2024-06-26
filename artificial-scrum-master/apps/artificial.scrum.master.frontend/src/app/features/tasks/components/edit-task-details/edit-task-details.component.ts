import { Component, EventEmitter, inject, input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { StoryTaskEditService } from '../../services/story-task-edit.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { TaskDetails } from '../../models/task-details';
import { EditorStateService } from '../../../../shared/services/editor-state.service';

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
  private readonly editorStateService = inject(EditorStateService);

  @Output() taskDetailsUpdate: EventEmitter<TaskDetails> = new EventEmitter();

  isEditorVisible = this.editorStateService.isEditorVisible;
  isSuggestionsVisible = this.editorStateService.isSuggestionsVisible;
  descriptionEditorValue = this.editorStateService.descriptionEditorValue;
  suggestionString = this.editorStateService.suggestionString;

  details = input.required<TaskDetails | null>();
  taskId = input.required<number>();
  isLoading = input.required<boolean>();

  updateDescription(newValue: string) {
    this.editorStateService.updateDescription(newValue);
  }

  rejectSuggestion() {
    this.editorStateService.setSuggestionsVisible(false);
  }

  replaceWithSuggestion() {
    if (this.details() == null) {
      return;
    }
    this.editorStateService.replaceWithSuggestion();
  }

  appendSuggestionToBack() {
    if (this.details() == null) {
      return;
    }
    this.editorStateService.appendSuggestionToBack();
  }

  appendSuggestionToFront() {
    if (this.details() == null) {
      return;
    }
    this.editorStateService.appendSuggestionToFront();
  }

  resetDescription() {
    this.editorStateService.resetDescription(this.details()?.description ?? '');
  }

  saveDescriptionChanges() {
    if (
      this.details()?.description ===
      this.editorStateService.descriptionEditorValue()
    ) {
      this.toastService.openError(
        this.translateService.instant('Tasks.Edit.NoChanges')
      );
      return;
    }
    this.storyTaskEditService
      .patchDescription(
        this.taskId(),
        this.details()?.version ?? 0,
        this.editorStateService.descriptionEditorValue()
      )
      .subscribe({
        next: (response) => {
          this.taskDetailsUpdate.emit(response);
          this.editorStateService.setDescriptionEditorValue(
            response.description ?? ''
          );
          this.toastService.openSuccess(
            this.translateService.instant('Tasks.UpdatedSuccessfully')
          );
          this.editorStateService.setEditorVisible(false);
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('Tasks.ErrorWhileUpdate')
          ),
      });
  }
}
