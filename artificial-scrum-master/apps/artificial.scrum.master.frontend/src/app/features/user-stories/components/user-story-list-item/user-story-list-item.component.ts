import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStory } from '../../models/user-story';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';

@Component({
  selector: 'app-user-story-list-item',
  standalone: true,
  templateUrl: './user-story-list-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    MatExpansionModule,
    MatSort,
    MatSortModule,
    MatTableModule,
    MatIconModule,
    AvatarComponent,
  ],
})
export class UserStoryListItemComponent {
  @Input()
  public userStory: UserStory;

  public panelOpenState = false;
}
