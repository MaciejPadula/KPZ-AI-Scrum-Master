import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectFeedComponent } from "./components/project-feed/project-feed.component";
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-project',
    standalone: true,
    templateUrl: './project.component.html',
    imports: [CommonModule, ProjectFeedComponent, TranslateModule]
})
export class ProjectComponent {}
