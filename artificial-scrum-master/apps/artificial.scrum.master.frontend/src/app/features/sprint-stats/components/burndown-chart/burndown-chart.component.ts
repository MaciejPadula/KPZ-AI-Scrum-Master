import { Component, computed, input, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SprintDayStats } from '../../models/GetSprintStats';
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
  imports: [CommonModule, NgApexchartsModule],
  templateUrl: './burndown-chart.component.html',
})
export class BurndownChartComponent {
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
          name: 'Real pending points',
          data: this.realPendingPoints(),
        },
        {
          name: 'Optimal pending points',
          data: this.optimalPendingPoints(),
        },
      ],
      chart: {
        id: 'yt',
        group: 'social',
        type: 'area',
        height: 240,
      },
      colors: ['#0078d4', 'lightgray'],
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
    },
  };
}
