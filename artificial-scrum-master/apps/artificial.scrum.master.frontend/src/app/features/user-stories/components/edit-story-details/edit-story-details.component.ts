import {
  Component,
  EventEmitter,
  inject,
  Input,
  input,
  Output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { StoryEditService } from '../../services/story-edit.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { UserStoryDetails } from '../../models/user-story-details';
import { EditorStateServiceService } from '../../../../shared/services/editor-state-service.service';

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

  @Input() editorStateServiceService: EditorStateServiceService;
  @Output() storyDetailsUpdate: EventEmitter<UserStoryDetails> =
    new EventEmitter();

  details = input.required<UserStoryDetails | null>();
  storyId = input.required<number>();
  isLoading = input.required<boolean>();

  updateDescription(newValue: string) {
    this.editorStateServiceService.descriptionEditorValue.set(newValue);
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
        this.translateService.instant('UserStories.Edit.NoChanges')
      );
      return;
    }

    this.storyTaskEditService
      .patchStoryDescription(
        this.storyId(),
        this.details()?.version ?? 0,
        this.editorStateServiceService.descriptionEditorValue()
      )
      .subscribe({
        next: (response) => {
          this.storyDetailsUpdate.emit(response);
          this.editorStateServiceService.descriptionEditorValue.set(
            response.description ?? ''
          );
          this.toastService.openSuccess(
            this.translateService.instant('UserStories.UpdatedSuccessfully')
          );
          this.editorStateServiceService.editorVisible = false;
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('UserStories.ErrorWhileUpdate')
          ),
      });
  }
}
