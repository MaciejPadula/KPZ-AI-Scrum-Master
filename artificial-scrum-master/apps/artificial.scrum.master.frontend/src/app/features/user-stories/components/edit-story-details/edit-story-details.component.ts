import { Component, inject, input, model } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { StoryEditService } from '../../services/story-edit.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { UserStoryDetails } from '../../models/user-story-details';
import { GetStorySuggestion } from '../../models/get-story-suggestion';

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
  private readonly storyTaskEditService = inject(StoryEditService);
  private readonly toastService = inject(ToastService);

  details = model.required<UserStoryDetails | null>();

  storyId = input.required<number>();
  isLoading = input.required<boolean>();

  isSuggestionsVisible = model.required<boolean>();
  suggestion = input.required<GetStorySuggestion | null>();

  isEditorVisible = model.required<boolean>();
  descriptionEditorValue = model.required<string>();

  updateDescription(newValue: string) {
    this.descriptionEditorValue.set(newValue);
  }

  rejectSuggestion() {
    this.isSuggestionsVisible.set(false);
  }

  replaceWithSuggestion() {
    if (this.details() == null) {
      return;
    }
    this.descriptionEditorValue.set(
      this.suggestion()?.descriptionEditSuggestion ?? ''
    );

    this.isSuggestionsVisible.set(false);
    if (!this.isEditorVisible()) {
      this.isEditorVisible.set(true);
    }
  }

  appendSuggestionToBack() {
    if (this.details() == null) {
      return;
    }
    this.descriptionEditorValue.set(
      this.descriptionEditorValue().concat(
        '\n',
        this.suggestion()?.descriptionEditSuggestion ?? ''
      )
    );

    this.isSuggestionsVisible.set(false);
    if (!this.isEditorVisible()) {
      this.isEditorVisible.set(true);
    }
  }

  appendSuggestionToFront() {
    if (this.details() == null) {
      return;
    }
    this.descriptionEditorValue.set(
      (this.suggestion()?.descriptionEditSuggestion ?? '').concat(
        '\n',
        this.descriptionEditorValue()
      )
    );

    this.isSuggestionsVisible.set(false);
    if (!this.isEditorVisible()) {
      this.isEditorVisible.set(true);
    }
  }

  resetDescription() {
    this.isEditorVisible.set(false);
    this.descriptionEditorValue.set(this.details()?.description ?? '');
  }

  saveDescriptionChanges() {
    if (this.details()?.description === this.descriptionEditorValue()) {
      this.toastService.openError(
        this.translateService.instant('UserStories.Edit.NoChanges')
      );
      return;
    }

    this.storyTaskEditService
      .patchStoryDescription(
        this.storyId(),
        this.details()?.version ?? 0,
        this.descriptionEditorValue()
      )
      .subscribe({
        next: (response) => {
          this.details.set(response);
          this.descriptionEditorValue.set(response.description ?? '');
          this.toastService.openSuccess(
            this.translateService.instant('UserStories.UpdatedSuccessfully')
          );
          this.isEditorVisible.set(false);
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('UserStories.ErrorWhileUpdate')
          ),
      });
  }
}
