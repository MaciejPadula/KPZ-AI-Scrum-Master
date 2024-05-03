import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../../shared/services/toast.service';
import { StoryTasksService } from '../../services/story-tasks.service';
import { TaskRow } from '../../../../shared/components/task-row/task-row';
import { TaskRowComponent } from '../../../../shared/components/task-row/task-row.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { finalize } from 'rxjs';
import { MaterialModule } from '../../../..//shared/material.module';

@Component({
  selector: 'app-story-task-list',
  standalone: true,
  imports: [CommonModule, TaskRowComponent, MaterialModule, TranslateModule],
  templateUrl: './story-task-list.component.html',
})
export class StoryTaskListComponent implements OnInit {
  @Input()
  public userStoryId: number | null;
  @Input()
  public sprintId: number | null;

  private readonly translateService = inject(TranslateService);
  private readonly sprintPreviewDataService = inject(StoryTasksService);
  private readonly toastService = inject(ToastService);

  #storyTasks = signal<TaskRow[]>([]);
  public readonly storyTasks = this.#storyTasks.asReadonly();

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  ngOnInit(): void {
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
