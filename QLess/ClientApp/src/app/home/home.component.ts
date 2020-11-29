import { Component, OnInit, Inject } from '@angular/core';
import { QLessModel } from '../entities/QLessModel';
import { HttpClient, HttpParams, HttpHeaders} from '@angular/common/http';
import { QLessViewModel } from '../entities/QLessViewModel';
import { PriceMatrix } from '../entities/PriceMatrix';
import { QLessRegistration } from '../entities/QLessRegistration';
import { CardType } from '../entities/enums';
import { diffBetweenDates, diffMonthsFromPurchaseDate } from '../util/DateUtil';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit  {
  public qLessResult: QLessModel;
  public qLessModel: QLessModel;
  public qLessViewModel: QLessViewModel;
  public qlessRegister: QLessRegistration;
  public priceMatrix: PriceMatrix;
  public cardId: number;
  baseUrl: string;
  httpClient: HttpClient;
  public isShow: boolean;
  public isEntry: boolean;
  public stnName: string;
  public isShowRegister: boolean;
  public  txtStatus: string;

  showRegisterBtn: boolean;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.priceMatrix = new PriceMatrix();
    this.qLessModel = new QLessModel();
    this.qLessModel.isEntry = !this.priceMatrix.stationFr;
    this.isShow = false;
    this.isEntry = !this.priceMatrix.stationFr;
    this.txtStatus = "";

    this.isShowRegister = false;
    this.qlessRegister = new QLessRegistration();
   }

  scanCard(): void {
    if (this.qLessModel.id  && this.qLessModel.id !== this.cardId) {
      this.ngOnInit();
      this.scanCard();
      return alert("Card used to enter is different. You will be in a new travel now");
    }
    let params = new HttpParams()
      .set("id", this.cardId.toString())
      .set("stnFrom", (this.priceMatrix.stationFr ? this.priceMatrix.stationFr.toString() : ''))
      .set("line", (this.priceMatrix.stationFr ? this.priceMatrix.line.toString() : ''))
    this.httpClient.get<QLessViewModel>(this.baseUrl + 'qless', { params }).subscribe(result => {

      this.qLessViewModel = result;
      this.priceMatrix = result.priceMatrix;
      this.qLessModel = result.qLessModel;
      this.qLessModel.active = diffBetweenDates(this.qLessModel.purchaseDate, new Date(), 'years') <= 5;
      this.stnName = this.qLessModel.isEntry ? this.priceMatrix.stationFrName : this.priceMatrix.stationToName;
      this.isShow = true;
      if (!this.qLessModel.isEntry) {
        this.isEntry = true; //reset for new entry
      } else {
        this.isEntry = false;
      }
      if (this.qLessModel.type === CardType.Regular) {
        this.showRegisterBtn = true;
      }
     
    }, error => console.error(error));
  }
  showRegister(): void {

    this.isShowRegister = true;
  }
  registerCard(): void {
    const errMsg = this.validRegister();
    if (errMsg === "") {
      this.qlessRegister.qLessCardSerialNo = this.qLessModel.serialNo;
      this.qlessRegister.qLessCardId = this.qLessModel.id;


      const headers = new HttpHeaders()
        .set('Content-Type', 'application/json;charset=UTF-8')
      let options = { headers: headers };

      this.httpClient.post<QLessViewModel>(this.baseUrl + 'qless', JSON.stringify(this.qlessRegister), options).subscribe(result => {
        if (result.status === 'success') {
          this.isShowRegister = false;
          alert("You are now registered, enjoy 20% discount upon your next trip, with additional 3% discount on succeding trips (max of 4 trips/day)");
        }
      }, error => console.error(error));
    } else {
      return alert(errMsg);
    }
 
  }
  validRegister(): string {

    var months = diffMonthsFromPurchaseDate(this.qLessModel.purchaseDate, new Date(), 'months');
    if (!this.qlessRegister.srCCN && !this.qlessRegister.pwdId) {
      return "Please fill out either Senior Citizen  Control Number or PWD ID Number";
    } else if (months > 6) {
      return "Your card can't be registered. Cards can only be registred within 6 months from date of purchase";
    }

    return "";
  }
}
