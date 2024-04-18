import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectFeedComponent } from './components/project-feed/project-feed.component';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { PokerSessionsListComponent } from "./components/poker-sessions-list/poker-sessions-list.component";

@Component({
    selector: 'app-project',
    standalone: true,
    templateUrl: './project.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [CommonModule, ProjectFeedComponent, TranslateModule, PokerSessionsListComponent]
})
export class ProjectComponent implements OnInit {
  private readonly activatedRoute = inject(ActivatedRoute);

  #projectId = signal<number>(0);
  public projectId = this.#projectId.asReadonly();

  public ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const projectId = params['projectId'];
      this.#projectId.set(projectId);
    });
  }
}
