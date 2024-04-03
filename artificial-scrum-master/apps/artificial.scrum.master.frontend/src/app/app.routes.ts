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
    path: 'Project',
    loadChildren: () =>
      import('./features/project/routes').then((mod) => mod.ProjectRoutes),
  },
];
