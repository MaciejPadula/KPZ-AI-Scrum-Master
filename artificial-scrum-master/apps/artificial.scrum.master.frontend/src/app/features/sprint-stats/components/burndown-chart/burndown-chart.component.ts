import { Component, computed, inject, input, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintDayStats } from '../../models/GetSprintStats';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { NgApexchartsModule, ApexOptions } from 'ng-apexcharts';
import { ThemeService } from 'apps/artificial.scrum.master.frontend/src/app/shared/services/theme.service';

@Component({
  selector: 'app-burndown-chart',
  standalone: true,
  imports: [CommonModule, TranslateModule, NgApexchartsModule],
  templateUrl: './burndown-chart.component.html',
})
export class BurndownChartComponent {
  private readonly translateService = inject(TranslateService);
  private readonly themeService = inject(ThemeService);

  private readonly textColor: Signal<string> = computed(() =>
    this.themeService.darkTheme() ? 'white' : 'black'
  );

  sprintDayStats = input.required<SprintDayStats[]>();

  realPendingPoints = computed(() =>
    this.sprintDayStats().map((elem) => [
      new Date(elem.day).getTime(),
      Number(elem.openPoints.toFixed(0)),
    ])
  );

  optimalPendingPoints = computed(() =>
    this.sprintDayStats().map((elem) => [
      new Date(elem.day).getTime(),
      Number(elem.optimalPoints.toFixed(0)),
    ])
  );

  burndownChartOptions: Signal<Partial<ApexOptions>> = computed(() => {
    return {
      series: [
        {
          name: this.translateService.instant('Stats.BurndownChart.Optimal'),
          data: this.optimalPendingPoints(),
        },
        {
          name: this.translateService.instant('Stats.BurndownChart.Real'),
          data: this.realPendingPoints(),
        },
      ],
      chart: {
        id: 'burn-down-chart',
        group: 'social',
        type: 'area',
        height: 240,
      },
      colors: ['lightgray', '#0078d4'],
      xaxis: {
        type: 'datetime',
        labels: {
          format: 'dd.MM',
          style: {
            colors: this.textColor(),
          },
        },
      },
      yaxis: {
        labels: {
          style: {
            colors: this.textColor(),
          },
        },
      },
      legend: {
        show: true,
        labels: {
          colors: this.textColor(),
        },
      },
    };
  });

  public commonOptions: Partial<ApexOptions> = {
    dataLabels: {
      enabled: false,
    },
    stroke: {
      curve: 'straight',
    },
    markers: {
      size: 5,
      hover: {
        size: 8,
      },
    },
    tooltip: {
      followCursor: false,
      theme: 'dark',
      x: {
        show: false,
      },
      marker: {
        show: false,
      },
    },
  };
}
