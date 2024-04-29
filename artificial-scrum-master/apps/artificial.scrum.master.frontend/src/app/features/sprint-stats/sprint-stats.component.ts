import {
  Component,
  computed,
  effect,
  inject,
  input,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintStatsService } from './services/sprint-stats.service';
import { GetSprintStats } from './models/GetSprintStats';
import { ToastService } from '../../shared/services/toast.service';
import { MaterialModule } from '../../shared/material.module';
import { BurndownChartComponent } from './components/burndown-chart/burndown-chart.component';

@Component({
  selector: 'app-sprint-stats',
  standalone: true,
  templateUrl: './sprint-stats.component.html',
  imports: [CommonModule, MaterialModule, BurndownChartComponent],
})
export class SprintStatsComponent {
  sprintStatsService = inject(SprintStatsService);
  toastService = inject(ToastService);

  projectId = input.required<number>();
  sprintId = input.required<number>();

  #stats = signal<GetSprintStats>({} as GetSprintStats);
  public readonly stats = this.#stats.asReadonly();

  public totalPoints = computed(() => {
    if (!this.stats().totalRolePoints) {
      return -1;
    }
    return this.stats().totalRolePoints.reduce(
      (p, rolePointsPair) => p + rolePointsPair.value,
      0
    );
  });

  constructor() {
    effect(() => {
      this.loadSprintStats(this.projectId(), this.sprintId());
    });
  }

  private loadSprintStats(projectId: number, sprintId: number): void {
    this.sprintStatsService.getSprintStats(projectId, sprintId).subscribe({
      next: (stats) => {
        this.#stats.set(stats);
      },
      error: () =>
        this.toastService.openError('Error fetching sprint statistics'),
    });
  }
}
