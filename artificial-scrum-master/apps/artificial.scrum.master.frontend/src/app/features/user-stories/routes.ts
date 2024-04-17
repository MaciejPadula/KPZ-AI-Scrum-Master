import { Route } from '@angular/router';
import { UserStoryFeedComponent } from './user-story-feed.component';

export const UserStoriesRoutes: Route[] = [
  {
    path: ':sprintId',
    component: UserStoryFeedComponent,
  },
];
