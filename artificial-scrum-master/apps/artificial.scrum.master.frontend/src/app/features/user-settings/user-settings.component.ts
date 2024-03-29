import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserSettingsDataService } from './services/user-settings-data.service';
import { UserSettings } from './models/user-settings';
import { TaigaAuthorizationService } from './services/taiga-authorization.service';
import { TaigaAuthorizationComponent } from './components/taiga-authorization/taiga-authorization.component';
import { MaterialModule } from '../../shared/material.module';
import { TaigaIconComponent } from '../../shared/components/taiga-icon/taiga-icon.component';

@Component({
  selector: 'app-user-settings',
  standalone: true,
  templateUrl: './user-settings.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    TaigaAuthorizationComponent,
    MaterialModule,
    TaigaIconComponent,
  ],
})
export class UserSettingsComponent implements OnInit {
  private readonly userSettingsDataService = inject(UserSettingsDataService);
  private readonly taigaAuthorizationService = inject(
    TaigaAuthorizationService
  );

  #settings = signal<UserSettings | null>(null);
  public settings = this.#settings.asReadonly();

  public ngOnInit(): void {
    this.userSettingsDataService.getUserSettings().subscribe((settings) => {
      this.#settings.set(settings);
      this.taigaAuthorizationService.loggedToTaiga = settings.isLoggedToTaiga;
    });
  }
}
