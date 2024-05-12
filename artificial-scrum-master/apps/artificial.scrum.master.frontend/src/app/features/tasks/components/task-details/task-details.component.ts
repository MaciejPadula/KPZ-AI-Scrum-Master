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
import { TranslateModule } from '@ngx-translate/core';
import { FormatDateService } from '../../../../shared/services/format-date.service';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { GetStoryTaskSuggestion } from '../../models/get-story-task-suggestion';
import { StoryTaskSuggestionService } from '../../services/story-task-suggestion.service';
import { finalize } from 'rxjs';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { EditTaskDetailsComponent } from '../edit-task-details/edit-task-details.component';

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
    EditTaskDetailsComponent,
  ],
})
export class TaskDetailsComponent implements OnInit {
  @ViewChild('taskDescription', { read: ElementRef })
  taskDescription: ElementRef;
  @ViewChild('taskEditor', { read: ElementRef })
  taskEditor: ElementRef;

  private readonly formatDateService = inject(FormatDateService);
  private readonly storyTaskSuggestionService = inject(
    StoryTaskSuggestionService
  );

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
  public readonly taskId: number;

  constructor(@Inject(MAT_DIALOG_DATA) taskId: number) {
    this.#taskId = taskId;
    this.taskId = taskId;
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
          setTimeout(() => this.scrollToElement(this.taskEditor), 50);
        })
      )
      .subscribe({
        next: (response) => this.suggestion.set(response),
        error: () => this.error.set(true),
      });
  }

  openDescriptionEditor() {
    if (this.isEditorVisible()) {
      setTimeout(() => this.isEditorVisible.set(false), 100);
      setTimeout(() => this.scrollToElement(this.taskDescription), 50);
      return;
    }
    setTimeout(() => this.isEditorVisible.set(true), 50);
    setTimeout(() => this.scrollToElement(this.taskEditor), 100);
  }

  private scrollToElement(element: ElementRef) {
    if (element && element.nativeElement) {
      element.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
      });
    }
  }
}
