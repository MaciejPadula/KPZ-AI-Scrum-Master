import { Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TimelineRow } from './timeline-row';
import { TranslateService } from '@ngx-translate/core';
import { ScrumObjectType } from '../../models/scrum-object-type';
import { ScrumObjectState } from '../../models/scrum-object-state';
import { AvatarComponent } from '../avatar/avatar.component';
import { LinkComponent } from '../link/link.component';
import { MaterialModule } from '../../material.module';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-timeline-row',
  standalone: true,
  templateUrl: './timeline-row.component.html',
  imports: [
    CommonModule,
    AvatarComponent,
    LinkComponent,
    MaterialModule,
    RouterModule,
  ],
})
export class TimelineRowComponent {
  private readonly translateService = inject(TranslateService);

  public timelineRow = input.required<TimelineRow>();
  public showProjectLink = input<boolean>(true);

  public getScrumObjectTypeText(type: ScrumObjectType) {
    return this.translateService.instant(
      `Shared.Timeline.Row.ScrumObjectType.${ScrumObjectType[type]}`
    );
  }

  public getHasBeenText(type: ScrumObjectType) {
    return this.translateService.instant(
      `Shared.Timeline.Row.HasBeen.${ScrumObjectType[type]}`
    );
  }

  public getObjectUrl(type: ScrumObjectType, id: number) {
    return `/${ScrumObjectType[type]}/${id}`;
  }

  public getStateText(type: ScrumObjectType, state: ScrumObjectState) {
    return this.translateService.instant(
      `Shared.Timeline.Row.ScrumObjectState.${ScrumObjectState[state]}.${ScrumObjectType[type]}`
    );
  }
}
