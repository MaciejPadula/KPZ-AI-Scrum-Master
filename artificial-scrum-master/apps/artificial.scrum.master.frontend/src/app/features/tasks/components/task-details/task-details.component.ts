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
import { StoryTaskSuggestionService } from '../../services/story-task-suggestion.service';
import { finalize } from 'rxjs';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { EditTaskDetailsComponent } from '../edit-task-details/edit-task-details.component';
import { EditorStateServiceService } from '../../../../shared/services/editor-state-service.service';

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
  private readonly editorStateServiceService = inject(
    EditorStateServiceService
  );

  details = signal<TaskDetails | null>(null);
  error = signal<boolean>(false);

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
        this.editorStateServiceService.setDescriptionEditorValue =
          response.description ?? '';
      },
      error: () => this.error.set(true),
    });
  }

  generateSuggestion() {
    if (this.details() == null || !this.details()?.subject) {
      return;
    }
    this.#isLoading.set(true);
    setTimeout(() => this.scrollToElement(this.taskEditor), 50);

    this.storyTaskSuggestionService
      .getTaskDescriptionSuggestion(
        this.details()?.subject ?? '',
        this.details()?.userStorySubject ?? null,
        this.details()?.description ?? null
      )
      .pipe(
        finalize(() => {
          this.#isLoading.set(false);
          this.editorStateServiceService.suggestionsVisible = true;
          setTimeout(() => this.scrollToElement(this.taskEditor), 50);
        })
      )
      .subscribe({
        next: (response) =>
          (this.editorStateServiceService.setSuggestionString =
            response.descriptionEditSuggestion),
        error: () => this.error.set(true),
      });
  }

  toggleDescriptionEditor() {
    if (this.editorStateServiceService.isEditorVisible()) {
      this.editorStateServiceService.editorVisible = false;
      this.scrollToElement(this.taskDescription, 'start');
      return;
    }
    this.editorStateServiceService.editorVisible = true;
    setTimeout(() => this.scrollToElement(this.taskEditor), 50);
  }

  updateTaskDetails($event: TaskDetails) {
    this.details.set($event);
  }

  private scrollToElement(element: ElementRef, block: 'end' | 'start' = 'end') {
    if (element && element.nativeElement) {
      element.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: block,
        inline: 'nearest',
      });
    }
  }
}
