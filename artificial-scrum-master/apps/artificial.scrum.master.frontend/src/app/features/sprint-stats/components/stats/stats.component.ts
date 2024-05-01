import { Component, computed, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GetSprintStats } from '../../models/GetSprintStats';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-stats',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './stats.component.html',
})
export class StatsComponent {
  stats = input.required<GetSprintStats>();

  private readonly rolePointsNotSet = -1;

  public totalPoints = computed(() => {
    if (!this.stats().totalRolePoints) {
      return this.rolePointsNotSet;
    }
    return this.stats().totalRolePoints.reduce(
      (p, rolePointsPair) => p + rolePointsPair.value,
      0
    );
  });
}
