import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStoryListComponent } from './components/user-story-list/user-story-list.component';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-user-story-feed',
  standalone: true,
  templateUrl: './user-story-feed.component.html',
  imports: [CommonModule, UserStoryListComponent, TranslateModule],
})
export class UserStoryFeedComponent {}
