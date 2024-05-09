import {
  Component,
  computed,
  inject,
  Inject,
  OnInit,
  signal,
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
  private readonly formatDateService = inject(FormatDateService);

  details = signal<TaskDetails | null>(null);
  error = signal<boolean>(false);

  isSuggestionsVisible = signal(false);
  suggestion = signal<GetStoryTaskSuggestion | null>(null);
  storyTaskSuggestionService = inject(StoryTaskSuggestionService);

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
      next: (response) => this.details.set(response),
      error: () => this.error.set(true),
    });
  }

  generateSuggestion() {
    if (this.details() == null || !this.details()?.subject) {
      return;
    }
    this.storyTaskSuggestionService
      .getTaskDescriptionSuggestion(
        this.details()?.subject ?? '',
        this.details()?.userStorySubject ?? null,
        this.details()?.description ?? null
      )
      .pipe(finalize(() => this.isSuggestionsVisible.set(true)))
      .subscribe({
        next: (response) => this.suggestion.set(response),
        error: () => this.error.set(true),
      });
  }

  updateDescription(newValue: string) {
    console.log(newValue);
  }
}
