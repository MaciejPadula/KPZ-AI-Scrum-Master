import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedListComponent } from "./components/feed-list/feed-list.component";
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-feed',
    standalone: true,
    templateUrl: './feed.component.html',
    imports: [CommonModule, FeedListComponent, TranslateModule],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FeedComponent {}
