import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectFeedComponent } from './components/project-feed/project-feed.component';
import { TranslateModule } from '@ngx-translate/core';
import { SprintPreviewComponent } from './components/sprint-preview/sprint-preview.component';
@Component({
  selector: 'app-project',
  standalone: true,
  templateUrl: './project.component.html',
  imports: [
    CommonModule,
    ProjectFeedComponent,
    TranslateModule,
    SprintPreviewComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectComponent {}
