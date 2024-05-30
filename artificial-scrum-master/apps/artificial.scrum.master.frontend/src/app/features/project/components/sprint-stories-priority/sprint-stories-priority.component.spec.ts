import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SprintStoriesPriorityComponent } from './sprint-stories-priority.component';

describe('SprintStoriesPriorityComponent', () => {
  let component: SprintStoriesPriorityComponent;
  let fixture: ComponentFixture<SprintStoriesPriorityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SprintStoriesPriorityComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SprintStoriesPriorityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
