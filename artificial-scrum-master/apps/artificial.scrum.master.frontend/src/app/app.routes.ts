import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: 'UserSettings',
    loadChildren: () =>
      import('./features/user-settings/routes').then(
        (mod) => mod.UserSettingsRoutes
      ),
  },
];
