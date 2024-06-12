import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { StoryTasksService } from './services/story-tasks.service';
import { ToastService } from '../../shared/services/toast.service';
import { TaskRow } from './models/task-row';
import { finalize } from 'rxjs';
import { TaskRowComponent } from './components/task-row/task-row.component';
import { MaterialModule } from '../../shared/material.module';
import { StoryStateService } from '../user-stories/services/story-state.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-tasks',
  standalone: true,
  imports: [CommonModule, TaskRowComponent, MaterialModule, TranslateModule],
  templateUrl: './tasks.component.html',
})
export class TasksComponent implements OnInit {
  @Input()
  public userStoryId: number | null;
  @Input()
  public sprintId: number | null;

  private readonly translateService = inject(TranslateService);
  private readonly sprintPreviewDataService = inject(StoryTasksService);
  private readonly toastService = inject(ToastService);
  private readonly storyStateService = inject(StoryStateService);

  #storyTasks = signal<TaskRow[]>([]);
  public readonly storyTasks = this.#storyTasks.asReadonly();

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  constructor() {
    this.storyStateService.refresh$
      .pipe(takeUntilDestroyed())
      .subscribe(() => this.loadStoryTasks());
  }

  ngOnInit(): void {
    this.loadStoryTasks();
  }

  private loadStoryTasks(): void {
    this.#isLoading.set(true);

    this.sprintPreviewDataService
      .getStoryTasks(this.userStoryId, this.sprintId)
      .pipe(finalize(() => this.#isLoading.set(false)))
      .subscribe({
        next: (stories) => {
          this.#storyTasks.set(stories);
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('UserStories.Tasks.Error')
          ),
      });
  }
}
