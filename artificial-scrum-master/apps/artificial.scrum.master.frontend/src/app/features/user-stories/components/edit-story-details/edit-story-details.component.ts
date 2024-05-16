import { Component, EventEmitter, inject, input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { StoryEditService } from '../../services/story-edit.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { UserStoryDetails } from '../../models/user-story-details';
import { EditorStateService } from '../../../../shared/services/editor-state.service';

@Component({
  selector: 'app-edit-story-details',
  standalone: true,
  imports: [
    CommonModule,
    MaterialModule,
    TranslateModule,
    DescriptionDiffComponent,
    MarkdownEditorComponent,
  ],
  templateUrl: './edit-story-details.component.html',
})
export class EditStoryDetailsComponent {
  private readonly translateService = inject(TranslateService);
  private readonly storyEditService = inject(StoryEditService);
  private readonly toastService = inject(ToastService);
  private readonly editorStateService = inject(EditorStateService);

  @Output() storyDetailsUpdate: EventEmitter<UserStoryDetails> =
    new EventEmitter();

  isEditorVisible = this.editorStateService.isEditorVisible;
  isSuggestionsVisible = this.editorStateService.isSuggestionsVisible;
  descriptionEditorValue = this.editorStateService.descriptionEditorValue;
  suggestionString = this.editorStateService.suggestionString;

  details = input.required<UserStoryDetails | null>();
  storyId = input.required<number>();
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
        this.translateService.instant('UserStories.Edit.NoChanges')
      );
      return;
    }
    this.storyEditService
      .patchDescription(
        this.storyId(),
        this.details()?.version ?? 0,
        this.editorStateService.descriptionEditorValue()
      )
      .subscribe({
        next: (response) => {
          this.storyDetailsUpdate.emit(response);
          this.editorStateService.descriptionEditorValue;
          response.description ?? '';
          this.toastService.openSuccess(
            this.translateService.instant('UserStories.UpdatedSuccessfully')
          );
          this.editorStateService.setEditorVisible(false);
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('UserStories.ErrorWhileUpdate')
          ),
      });
  }
}
