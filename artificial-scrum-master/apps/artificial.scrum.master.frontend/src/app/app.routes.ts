import { Route } from '@angular/router';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

export const appRoutes: Route[] = [
  {
    path: 'UserSettings',
    loadChildren: () =>
      import('./features/user-settings/routes').then(
        (mod) => mod.UserSettingsRoutes
      ),
  },
  {
    path: '',
    loadChildren: () =>
      import('./features/feed/routes').then((mod) => mod.FeedRoutes),
  },
  {
    path: 'Project',
    loadChildren: () =>
      import('./features/project/routes').then((mod) => mod.ProjectRoutes),
  },
  {
    path: 'Projects',
    loadChildren: () =>
      import('./features/projects/routes').then((mod) => mod.ProjectsRoutes),
  },
  {
    path: 'EstimationPoker',
    loadChildren: () =>
      import('./features/estimation-poker/routes').then((mod) => mod.EstimationPokerRoutes),
  },
  {
    path: '**',
    component: NotFoundComponent,
  },
];
