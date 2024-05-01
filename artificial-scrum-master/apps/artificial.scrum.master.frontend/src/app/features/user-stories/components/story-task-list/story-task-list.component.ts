import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../../shared/services/toast.service';
import { StoryTasksService } from '../../services/story-tasks.service';
import { TaskRow } from '../../../../shared/components/task-row/task-row';
import { TaskRowComponent } from '../../../../shared/components/task-row/task-row.component';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-story-task-list',
  standalone: true,
  imports: [CommonModule, TaskRowComponent],
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

  ngOnInit(): void {
    this.sprintPreviewDataService
      .getStoryTasks(this.userStoryId, this.sprintId)
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
