import {
  Component,
  computed,
  ElementRef,
  inject,
  Inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaskDetails } from '../../models/task-details';
import { HttpClient } from '@angular/common/http';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { FormatDateService } from '../../../../shared/services/format-date.service';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { GetStoryTaskSuggestion } from '../../models/get-story-task-suggestion';
import { StoryTaskSuggestionService } from '../../services/story-task-suggestion.service';
import { finalize } from 'rxjs';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { StoryTaskEditService } from '../../services/story-task-edit.service';
import { ToastService } from '../../../../shared/services/toast.service';

@Component({
  selector: 'app-task-details',
  standalone: true,
  templateUrl: './task-details.component.html',
  styleUrls: ['./task-details.component.scss'],
  imports: [
    CommonModule,
    MaterialModule,
    TranslateModule,
    DescriptionDiffComponent,
    MarkdownEditorComponent,
  ],
})
export class TaskDetailsComponent implements OnInit {
  @ViewChild('taskDescription', { read: ElementRef })
  taskDescription: ElementRef;
  @ViewChild('descriptionDiff', { read: ElementRef })
  descriptionDiff: ElementRef;
  @ViewChild('editor', { read: ElementRef }) editor: ElementRef;

  private readonly translateService = inject(TranslateService);
  private readonly formatDateService = inject(FormatDateService);
  private readonly storyTaskSuggestionService = inject(
    StoryTaskSuggestionService
  );
  private readonly storyTaskEditService = inject(StoryTaskEditService);
  private readonly toastService = inject(ToastService);

  details = signal<TaskDetails | null>(null);
  error = signal<boolean>(false);

  isSuggestionsVisible = signal(false);
  suggestion = signal<GetStoryTaskSuggestion | null>(null);

  isEditorVisible = signal(false);
  descriptionEditorValue = signal<string>('');

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  formattedDate = computed(() =>
    this.formatDateService.formatDate(this.details()?.createdDate)
  );

  #httpClient = inject(HttpClient);
  #taskId: number;

  constructor(@Inject(MAT_DIALOG_DATA) taskId: number) {
    this.#taskId = taskId;
  }

  ngOnInit(): void {
    this.#httpClient.get<TaskDetails>(`/api/task/${this.#taskId}`).subscribe({
      next: (response) => {
        this.details.set(response);
        this.descriptionEditorValue.set(response.description ?? '');
      },
      error: () => this.error.set(true),
    });
  }

  generateSuggestion() {
    if (this.details() == null || !this.details()?.subject) {
      return;
    }
    this.#isLoading.set(true);

    this.storyTaskSuggestionService
      .getTaskDescriptionSuggestion(
        this.details()?.subject ?? '',
        this.details()?.userStorySubject ?? null,
        this.details()?.description ?? null
      )
      .pipe(
        finalize(() => {
          this.#isLoading.set(false);
          this.isSuggestionsVisible.set(true);
          setTimeout(() => this.scrollToElement(this.descriptionDiff), 50);
        })
      )
      .subscribe({
        next: (response) => this.suggestion.set(response),
        error: () => this.error.set(true),
      });
  }

  updateDescription(newValue: string) {
    console.log(newValue);
    this.descriptionEditorValue.set(newValue);
  }

  openDescriptionEditor() {
    if (this.isEditorVisible()) {
      setTimeout(() => this.isEditorVisible.set(false), 100);
      setTimeout(() => this.scrollToElement(this.taskDescription), 50);
      return;
    }
    setTimeout(() => this.isEditorVisible.set(true), 50);
    setTimeout(() => this.scrollToElement(this.editor), 100);
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
    setTimeout(() => this.scrollToElement(this.taskDescription), 50);
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
        this.#taskId,
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
          setTimeout(() => this.scrollToElement(this.taskDescription), 50);
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('Tasks.ErrorWhileUpdate')
          ),
      });
  }

  private scrollToElement(element: ElementRef) {
    if (element && element.nativeElement) {
      element.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
      });
      // nie działa
      element.nativeElement.focus();
    }
  }
}
