import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Output,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { EstimationPokerService } from '../../services/estimation-poker.service';
import { SelectedUserStory } from '../../models/selected-user-story';
import { UserStoryPreview } from '../../../sprints/models/sprint';
import { StoryDetailsDataService } from '../../../user-stories/services/story-details-data.service';
import { finalize, map } from 'rxjs';
import { MaterialModule } from 'apps/artificial.scrum.master.frontend/src/app/shared/material.module';

@Component({
  selector: 'app-user-story-selector',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './user-story-selector.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserStorySelectorComponent {
  private readonly pokerService = inject(EstimationPokerService);
  private readonly storyDetailsService = inject(StoryDetailsDataService);

  public availableUserStories = this.pokerService.userStories;

  @Output()
  public userStorySelected = new EventEmitter<SelectedUserStory>();

  public selectUserStory(userStory: UserStoryPreview) {
    this.pokerService.isLoading.set(true);
    this.storyDetailsService
      .getStoryDetails(userStory.id)
      .pipe(
        map((details) => {
          return <SelectedUserStory>{
            name: userStory.name,
            description: details.description,
          };
        }),
        finalize(() => this.pokerService.isLoading.set(false))
      )
      .subscribe((selectedUserStory) =>
        this.userStorySelected.emit(selectedUserStory)
      );
  }
}
