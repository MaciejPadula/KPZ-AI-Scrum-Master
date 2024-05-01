import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStory } from '../../models/user-story';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { StoryTaskListComponent } from '../story-task-list/story-task-list.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-user-story-list-item',
  standalone: true,
  templateUrl: './user-story-list-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    AvatarComponent,
    StoryTaskListComponent,
    MaterialModule,
    TranslateModule,
  ],
})
export class UserStoryListItemComponent {
  @Input()
  public userStory: UserStory;

  public panelOpenState = false;
}
