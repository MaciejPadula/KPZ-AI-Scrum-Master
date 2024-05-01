import { Route } from '@angular/router';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { authGuard } from './guards/auth.guard';
import { WelcomeComponent } from './features/welcome/welcome.component';

export const appRoutes: Route[] = [
  {
    path: 'UserSettings',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/user-settings/routes').then((mod) => mod.UserSettingsRoutes),
  },
  {
    path: '',
    component: WelcomeComponent
  },
  {
    path: 'Home',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/feed/routes').then((mod) => mod.FeedRoutes),
  },
  {
    path: 'Project',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/project/routes').then((mod) => mod.ProjectRoutes),
  },
  {
    path: 'Projects',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/projects/routes').then((mod) => mod.ProjectsRoutes),
  },
  {
    path: 'EstimationPoker',
    // canActivate: [authGuard],
    loadChildren: () =>
      import('./features/estimation-poker/routes').then((mod) => mod.EstimationPokerRoutes),
  },
  {
    path: 'UserStories',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/user-stories/routes').then(
        (mod) => mod.UserStoriesRoutes
      ),
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];
