import { Route } from "@angular/router";
import { RetrospectiveComponent } from "./retrospective.component";

export const RetrospectiveRoutes: Route[] = [
  {
    path: ':sessionId',
    component: RetrospectiveComponent,
  }
];
