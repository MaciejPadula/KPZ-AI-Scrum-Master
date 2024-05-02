import {
  Component,
  Input,
  ChangeDetectionStrategy,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStory } from '../../models/user-story';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { StoryTaskListComponent } from '../story-task-list/story-task-list.component';
import { MaterialModule } from '../../../../shared/material.module';
import { MatDialog } from '@angular/material/dialog';
import { UserStoryDetailsComponent } from '../user-story-details/user-story-details.component';
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

  #dialog = inject(MatDialog);

  public openDetails(): void {
    this.#dialog.open(UserStoryDetailsComponent, {
      data: this.userStory.userStoryId,
    });
  }
}
