import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { EstimationPokerService } from './services/estimation-poker.service';
import { ScrumMasterViewComponent } from './components/scrum-master-view/scrum-master-view.component';
import { DeveloperViewComponent } from './components/developer-view/developer-view.component';
import { TranslateModule } from '@ngx-translate/core';
import { SessionTaskComponent } from "./components/session-task/session-task.component";

@Component({
    selector: 'app-estimation-poker',
    standalone: true,
    templateUrl: './estimation-poker.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        CommonModule,
        ScrumMasterViewComponent,
        DeveloperViewComponent,
        TranslateModule,
        SessionTaskComponent
    ]
})
export class EstimationPokerComponent implements OnInit {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly estimationPokerService = inject(EstimationPokerService);

  public readonly showScrumMasterView =
    this.estimationPokerService.showScrumMasterView;
  public currentTask = this.estimationPokerService.sessionTask;

  #sessionId = signal<string>('');
  public sessionId = this.#sessionId.asReadonly();

  public ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      const sessionId = params['sessionId'];
      this.#sessionId.set(sessionId);
      this.estimationPokerService.loadSession(sessionId);
    });
  }
}
