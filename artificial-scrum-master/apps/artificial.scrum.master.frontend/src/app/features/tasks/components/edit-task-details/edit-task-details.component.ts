import { Component, inject, input, model } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { StoryTaskEditService } from '../../services/story-task-edit.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { GetStoryTaskSuggestion } from '../../models/get-story-task-suggestion';
import { TaskDetails } from '../../models/task-details';

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

  details = model.required<TaskDetails | null>();

  taskId = input.required<number>();
  isLoading = input.required<boolean>();

  isSuggestionsVisible = model.required<boolean>();
  suggestion = input.required<GetStoryTaskSuggestion | null>();

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
        this.translateService.instant('Tasks.Edit.NoChanges')
      );
      return;
    }

    this.storyTaskEditService
      .patchTaskDescription(
        this.taskId(),
        this.details()?.version ?? 0,
        this.descriptionEditorValue()
      )
      .subscribe({
        next: (response) => {
          this.details.set(response);
          this.descriptionEditorValue.set(response.description ?? '');
          this.toastService.openSuccess(
            this.translateService.instant('Tasks.UpdatedSuccessfully')
          );
          this.isEditorVisible.set(false);
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('Tasks.ErrorWhileUpdate')
          ),
      });
  }
}
