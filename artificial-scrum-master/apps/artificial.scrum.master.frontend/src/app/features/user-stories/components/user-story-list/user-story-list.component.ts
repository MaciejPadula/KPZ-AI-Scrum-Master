import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStoriesService } from '../../services/user-stories.service';
import { ActivatedRoute } from '@angular/router';
import { ToastService } from '../../../../shared/services/toast.service';
import { UserStory } from '../../models/user-story';
import { UserStoryListItemComponent } from '../user-story-list-item/user-story-list-item.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateService } from '@ngx-translate/core';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-user-story-list',
  standalone: true,
  imports: [CommonModule, UserStoryListItemComponent, MaterialModule],
  templateUrl: './user-story-list.component.html',
})
export class UserStoryListComponent implements OnInit {
  #projectId = signal<number>(0);
  public projectId = this.#projectId.asReadonly();
  private sprintId: number;

  private readonly translateService = inject(TranslateService);
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly sprintPreviewDataService = inject(UserStoriesService);
  private readonly toastService = inject(ToastService);

  #userStories = signal<UserStory[]>([]);
  public readonly userStories = this.#userStories.asReadonly();
  public sprintName: string;

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  ngOnInit(): void {
    this.#isLoading.set(true);

    this.activatedRoute.params.subscribe((params) => {
      this.sprintId = +params['sprintId'];
    });
    this.activatedRoute.queryParams.subscribe((params) => {
      this.#projectId.set(+params['project']);
    });

    this.loadUserStories(this.#projectId(), this.sprintId);
  }

  private loadUserStories(projectId: number, sprintId: number) {
    this.sprintPreviewDataService
      .getUserStories(projectId, sprintId)
      .pipe(finalize(() => this.#isLoading.set(false)))
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
