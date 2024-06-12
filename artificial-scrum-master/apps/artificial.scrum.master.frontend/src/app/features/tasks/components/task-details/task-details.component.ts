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
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { FormatDateService } from '../../../../shared/services/format-date.service';
import { DescriptionDiffComponent } from '../../../../shared/components/description-diff/description-diff.component';
import { StoryTaskSuggestionService } from '../../services/story-task-suggestion.service';
import { finalize } from 'rxjs';
import { MarkdownEditorComponent } from '../../../../shared/components/markdown-editor/markdown-editor.component';
import { EditTaskDetailsComponent } from '../edit-task-details/edit-task-details.component';
import { EditorStateService } from '../../../../shared/services/editor-state.service';
import { TaskDetailsDataService } from '../../services/task-details-data.service';
import { ScrollService } from '../../../../shared/services/scroll.service';

@Component({
  selector: 'app-task-details',
  standalone: true,
  templateUrl: './task-details.component.html',
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

  private readonly taskDetailsDataService = inject(TaskDetailsDataService);
  private readonly formatDateService = inject(FormatDateService);
  private readonly taskSuggestionService = inject(StoryTaskSuggestionService);
  private readonly editorStateService = inject(EditorStateService);
  private readonly scrollService = inject(ScrollService);

  details = signal<TaskDetails | null>(null);
  error = signal<boolean>(false);

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  formattedDate = computed(() =>
    this.formatDateService.formatDate(this.details()?.createdDate)
  );

  #taskId: number;
  public readonly taskId: number;

  constructor(@Inject(MAT_DIALOG_DATA) taskId: number) {
    this.#taskId = taskId;
    this.taskId = taskId;
  }

  ngOnInit(): void {
    this.taskDetailsDataService.getTaskDetails(this.#taskId).subscribe({
      next: (response) => {
        this.details.set(response);
        this.editorStateService.setDescriptionEditorValue(
          response.description ?? ''
        );
      },
      error: () => this.error.set(true),
    });
  }

  generateSuggestion() {
    if (this.details() == null || !this.details()?.subject) {
      return;
    }
    this.#isLoading.set(true);
    this.scrollService.scrollToElement({
      element: this.taskEditor,
      timeout: 100,
    });
    this.taskSuggestionService
      .getTaskDescriptionSuggestion(
        this.details()?.subject ?? '',
        this.details()?.userStorySubject ?? null,
        this.details()?.description ?? null
      )
      .pipe(
        finalize(() => {
          this.#isLoading.set(false);
          this.editorStateService.setSuggestionsVisible(true);
          this.scrollService.scrollToElement({
            element: this.taskEditor,
            timeout: 100,
          });
        })
      )
      .subscribe({
        next: (response) =>
          this.editorStateService.setSuggestionString(
            response.descriptionEditSuggestion
          ),
        error: () => this.error.set(true),
      });
  }

  toggleDescriptionEditor() {
    if (this.editorStateService.isEditorVisible()) {
      this.editorStateService.setEditorVisible(false);
      this.scrollService.scrollToElement({
        element: this.taskDescription,
        block: 'start',
      });
      return;
    }
    this.editorStateService.setEditorVisible(true);
    this.scrollService.scrollToElement({
      element: this.taskEditor,
      timeout: 100,
    });
  }

  updateTaskDetails(detailsUpdate: TaskDetails) {
    this.details.set(detailsUpdate);
  }
}
