import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStoriesService } from '../../services/user-stories.service';
import { ActivatedRoute } from '@angular/router';
import { ToastService } from '../../../../shared/services/toast.service';
import { UserStory } from '../../models/user-story';
import { UserStoryListItemComponent } from '../user-story-list-item/user-story-list-item.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-user-story-list',
  standalone: true,
  imports: [CommonModule, UserStoryListItemComponent, MaterialModule],
  templateUrl: './user-story-list.component.html',
})
export class UserStoryListComponent implements OnInit {
  private projectId: number;
  private sprintId: number;

  private readonly translateService = inject(TranslateService);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly sprintPreviewDataService = inject(UserStoriesService);
  private readonly toastService = inject(ToastService);

  #userStories = signal<UserStory[]>([]);
  public readonly userStories = this.#userStories.asReadonly();
  public sprintName: string;

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.sprintId = +params['sprintId'];
    });
    this.activatedRoute.queryParams.subscribe((params) => {
      this.projectId = +params['project'];
    });

    this.loadUserStories(this.projectId, this.sprintId);
  }

  private loadUserStories(projectId: number, sprintId: number) {
    this.sprintPreviewDataService
      .getUserStories(projectId, sprintId)
      .subscribe({
        next: (stories) => {
          this.#userStories.set(stories);
          this.sprintName =
            stories.length > 0 ? stories[0].sprintName : 'Sprint';
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('UserStories.Error')
          ),
      });
  }
}
