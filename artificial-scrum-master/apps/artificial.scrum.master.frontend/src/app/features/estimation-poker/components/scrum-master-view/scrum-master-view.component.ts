import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  inject,
  input,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { map, timer } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EstimationPokerService } from '../../services/estimation-poker.service';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { AddTaskDialogComponent } from '../add-task-dialog/add-task-dialog.component';
import { SessionTaskComponent } from '../session-task/session-task.component';
import { SuggestEstimationComponent } from "../suggest-estimation/suggest-estimation.component";
import { UsersListService } from '../../services/users-list.service';

@Component({
    selector: 'app-scrum-master-view',
    standalone: true,
    templateUrl: './scrum-master-view.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        CommonModule,
        MaterialModule,
        TranslateModule,
        SessionTaskComponent,
        SuggestEstimationComponent
    ]
})
export class ScrumMasterViewComponent implements OnInit{
  private readonly estimationPokerService = inject(EstimationPokerService);
  private readonly usersListService = inject(UsersListService);
  private readonly dialog = inject(MatDialog);

  public sessionId = input.required<string>();
  public currentTask = this.estimationPokerService.sessionTask;
  public estimations = this.estimationPokerService.taskEstimations;
  public averageEstimation = computed(() => this.estimationPokerService.averageTaskEstimation().toFixed(2));
  public estimationValues = computed(() => this.estimations().map(x => x.estimation));
  public taskId = computed(() => this.currentTask()?.id ?? 0);
  public users = this.usersListService.users;

  constructor() {
    timer(0, 5000)
      .pipe(
        takeUntilDestroyed(),
        map(() => this.estimationPokerService.loadSessionTask(this.sessionId()))
      )
      .subscribe();
  }

  public ngOnInit(): void {
    this.usersListService.connectScrumMaster(this.sessionId());
  }

  public addTask() {
    this.dialog
      .open(AddTaskDialogComponent, {
        data: this.sessionId(),
      })
      .afterClosed()
      .subscribe(() => {
        this.estimationPokerService.clearTaskEstimations();
      });
  }

  public revealEstimations() {
    const task = this.currentTask();
    if (task) {
      this.estimationPokerService.loadTaskEstimations(task.id);
    }
  }
}
