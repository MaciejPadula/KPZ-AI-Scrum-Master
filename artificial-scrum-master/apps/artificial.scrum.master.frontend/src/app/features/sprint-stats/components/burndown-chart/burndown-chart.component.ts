import { Component, computed, inject, input, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintDayStats } from '../../models/GetSprintStats';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import {
  NgApexchartsModule,
  ApexAxisChartSeries,
  ApexTitleSubtitle,
  ApexDataLabels,
  ApexFill,
  ApexMarkers,
  ApexYAxis,
  ApexXAxis,
  ApexTooltip,
  ApexStroke,
} from 'ng-apexcharts';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: any;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  title: ApexTitleSubtitle;
  fill: ApexFill;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  tooltip: ApexTooltip;
  stroke: ApexStroke;
  grid: any;
  colors: any;
  toolbar: any;
};

@Component({
  selector: 'app-burndown-chart',
  standalone: true,
  imports: [CommonModule, TranslateModule, NgApexchartsModule],
  templateUrl: './burndown-chart.component.html',
})
export class BurndownChartComponent {
  private readonly translateService = inject(TranslateService);

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

  burndownChartOptions: Signal<Partial<ChartOptions>> = computed(() => {
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
    };
  });

  public commonOptions: Partial<ChartOptions> = {
    dataLabels: {
      enabled: false,
    },
    stroke: {
      curve: 'straight',
    },
    toolbar: {
      tools: {
        selection: false,
      },
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
    grid: {
      clipMarkers: false,
    },
    xaxis: {
      type: 'datetime',
      labels: {
        format: 'dd.MM',
      },
    },
  };
}
