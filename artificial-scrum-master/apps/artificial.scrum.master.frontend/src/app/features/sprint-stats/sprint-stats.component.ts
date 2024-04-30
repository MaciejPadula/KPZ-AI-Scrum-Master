import { Component, effect, inject, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintStatsService } from './services/sprint-stats.service';
import { GetSprintStats } from './models/GetSprintStats';
import { ToastService } from '../../shared/services/toast.service';
import { BurndownChartComponent } from './components/burndown-chart/burndown-chart.component';
import { StatsComponent } from './components/stats/stats.component';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-sprint-stats',
  standalone: true,
  templateUrl: './sprint-stats.component.html',
  imports: [
    CommonModule,
    BurndownChartComponent,
    StatsComponent,
    TranslateModule,
  ],
})
export class SprintStatsComponent {
  sprintStatsService = inject(SprintStatsService);
  toastService = inject(ToastService);
  translateService = inject(TranslateService);

  projectId = input.required<number>();
  sprintId = input.required<number>();

  #stats = signal<GetSprintStats>({} as GetSprintStats);
  public readonly stats = this.#stats.asReadonly();

  constructor() {
    effect(() => {
      this.loadSprintStats(this.projectId(), this.sprintId());
    });
    console.log('SprintStatsComponent created');
  }

  private loadSprintStats(projectId: number, sprintId: number): void {
    this.sprintStatsService.getSprintStats(projectId, sprintId).subscribe({
      next: (stats) => {
        this.#stats.set(stats);
      },
      error: () =>
        this.toastService.openError(
          this.translateService.instant('Stats.Error')
        ),
    });
  }
}
