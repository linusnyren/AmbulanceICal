import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms'
import { DomSanitizer } from '@angular/platform-browser';
import { Observable, tap } from 'rxjs';
import { SchemaDataService } from './data/schema-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  schemaService: SchemaDataService;
  calendarFile$!: Observable<string>;
  calendarUrl!: string;
  reactiveForm = new FormGroup({
    schemaId: new FormControl('', [Validators.required]),
    vehicle: new FormControl('', [Validators.required]),
    team: new FormControl('', [Validators.required])
  })
  sanitizer: DomSanitizer;

  constructor(schemaService: SchemaDataService, sanitizer: DomSanitizer) {
    this.schemaService = schemaService;
    this.sanitizer = sanitizer;
  }

  onSubmit(): void{
    var values = this.reactiveForm.value;
    this.calendarFile$ = this.schemaService.test(values.schemaId, values.vehicle, values.team).pipe(
      tap(console.log)
    );
    this.calendarUrl = this.schemaService.getUrl(values.schemaId, values.vehicle, values.team);
    this.calendarUrl = this.calendarUrl.replace('http', 'webcal').replace(' ', '-');
  }
  sanitize(url:string){
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }
}
