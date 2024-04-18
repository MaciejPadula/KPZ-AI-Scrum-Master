import { Route } from '@angular/router';
import { EstimationPokerComponent } from './estimation-poker.component';
import { NotFoundComponent } from '../../shared/components/not-found/not-found.component';

export const EstimationPokerRoutes: Route[] = [
  {
    path: ':sessionId',
    component: EstimationPokerComponent,
  },
  {
    path: '**',
    component: NotFoundComponent,
  }
];
