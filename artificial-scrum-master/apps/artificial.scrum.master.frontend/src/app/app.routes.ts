import { Route } from '@angular/router';

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
    path: 'Projects',
    loadChildren: () =>
      import('./features/project-list/routes').then(
        (mod) => mod.ProjectsRoutes
      ),
  },
];
