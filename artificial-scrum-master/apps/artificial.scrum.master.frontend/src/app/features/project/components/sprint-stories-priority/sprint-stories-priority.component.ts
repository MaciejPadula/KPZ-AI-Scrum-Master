import { Component, inject, Inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { StoryPrioritySuggestion } from '../../models/story-priority-suggestion';
import { StoriesPrioritySuggestionService } from '../../services/stories-priority-suggestion.service';
import { MaterialModule } from '../../../../shared/material.module';
import {
  CdkDragDrop,
  CdkDropList,
  CdkDrag,
  moveItemInArray,
} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-sprint-stories-priority',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    TranslateModule,
    MaterialModule,
    CdkDropList,
    CdkDrag,
  ],
  templateUrl: './sprint-stories-priority.component.html',
  styleUrl: './sprint-stories-priority.component.scss',
})
export class SprintStoriesPriorityComponent implements OnInit {
  private readonly translateService = inject(TranslateService);
  private readonly storySuggestionService = inject(
    StoriesPrioritySuggestionService
  );

  private readonly sprintId: number;
  private readonly projectId: number;

  #storiesSuggestions = signal<StoryPrioritySuggestion[]>([]);
  public storiesSuggestions = this.#storiesSuggestions.asReadonly();

  #error = signal<boolean>(false);
  public error = this.#error.asReadonly();

  #isLoading = signal<boolean>(true);
  public isLoading = this.#isLoading.asReadonly();

  constructor(
    @Inject(MAT_DIALOG_DATA) data: { sprintId: number; projectId: number }
  ) {
    this.sprintId = data.sprintId;
    this.projectId = data.projectId;
  }

  ngOnInit(): void {
    this.storySuggestionService
      .getSuggestedStoriesPriority(this.projectId, this.sprintId)
      .subscribe({
        next: (suggestions) => {
          this.#storiesSuggestions.set(suggestions);
          this.#isLoading.set(false);
        },
        error: () => {
          this.#error.set(true);
          this.#isLoading.set(false);
        },
      });
  }

  generateSuggestionAnew() {
    this.#isLoading.set(true);
    this.storySuggestionService
      .getSuggestedStoriesPriority(this.projectId, this.sprintId, true)
      .subscribe({
        next: (suggestions) => {
          this.#storiesSuggestions.set(suggestions);
          this.#isLoading.set(false);
        },
        error: () => {
          this.#error.set(true);
          this.#isLoading.set(false);
        },
      });
  }

  drop(event: CdkDragDrop<StoryPrioritySuggestion[]>) {
    moveItemInArray(
      this.storiesSuggestions(),
      event.previousIndex,
      event.currentIndex
    );
  }
}
