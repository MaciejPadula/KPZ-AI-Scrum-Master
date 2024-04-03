import { Route } from '@angular/router';
import { ProjectListComponent } from './project-list.component';

export const ProjectsRoutes: Route[] = [
  {
    path: '',
    component: ProjectListComponent,
  },
];
