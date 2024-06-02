//  $(document).ready(function () {
//    let eventsArray = [];

//  $('#addButton').click(function () {
//    const eventValue = $('#eventInput').val().trim();
//  const eventValidation = $('#eventValidation');

//  if (eventValue) {
//    eventsArray.push(eventValue);
//  updateAccordionContent();
//  $('#eventInput').val('');
//  eventValidation.text('');
//    } else {
//    eventValidation.text('Please enter a valid event.');
//    }
//  });

//  function updateAccordionContent() {
//    $('#tablecontent').empty();
//    eventsArray.forEach((event, index) => {
//      var html = '';
//  html += '<tr>';
//    html += '<td class="typtxt font-weight-bold" name="typtxt" id="TypeVal">' + (index + 1) + '</td>';
//    html += '<td class="filenames" data-fileid="' + index + '">' + event + '</td>';
//    html += '<td><a class="btn btn-danger delete-btn" data-toggle="tooltip" data-placement="top" title="Delete"><i class="bx bx-message-square-x"></i></a></td>';
//    html += '</tr>';
//  $('#tablecontent').append(html);
//    });
//    updateHiddenEvent();
//  }

//    function updateHiddenEvent() {
//      $('#eventsArrayInput').val(JSON.stringify(eventsArray));
//    }

//  $('#tablecontent').on('click', '.delete-btn', function () {
//    var index = $(this).closest('tr').index();
//  eventsArray.splice(index, 1);
//  updateAccordionContent();
//  });
//  let RiskSourceArray = [];
//  $('#addRiskSource').click(function () {
//    const RiskSource = $('#RiskSourceInput').val().trim();
//  const RiskSourceValidation = $('#RiskSourceValidation');

//  if (RiskSource) {
//    RiskSourceArray.push(RiskSource);
//  updateRiskSourceAccordionContent();
//  $('#RiskSourceInput').val('');
//  RiskSourceValidation.text('');
//    } else {
//    RiskSourceValidation.text('Please enter a valid event.');
//    }
//  });

//  function updateRiskSourceAccordionContent() {
//    $('#RiskSourcetablecontent').empty();
//    RiskSourceArray.forEach((event, index) => {
//      var html = '';
//  html += '<tr>';
//    html += '<td class="typtxt font-weight-bold" name="typtxt" id="TypeVal">' + (index + 1) + '</td>';
//    html += '<td class="filenames" data-fileid="' + index + '">' + event + '</td>';
//    html += '<td><a class="btn btn-danger delete-btn" data-toggle="tooltip" data-placement="top" title="Delete"><i class="bx bx-message-square-x"></i></a></td>';
//    html += '</tr>';
//  $('#RiskSourcetablecontent').append(html);
//    });
//    updateHiddenRiskSource();
//  }

//  $('#RiskSourcetablecontent').on('click', '.delete-btn', function () {
//    var index = $(this).closest('tr').index();
//  RiskSourceArray.splice(index, 1);
//  updateRiskSourceAccordionContent();
//  });

//    function updateHiddenRiskSource() {
//      $('#RiskSourceArrayInput').val(JSON.stringify(RiskSourceArray));
//    }


//  let RiskCauseArray = [];
//  $('#addRiskCause').click(function () {
//    const RiskCause = $('#RiskCauseInput').val().trim();
//  const RiskCauseValidation = $('#RiskCauseValidation');

//  if (RiskCause) {
//    RiskCauseArray.push(RiskCause);
//  updateRiskCauseAccordionContent();
//  $('#RiskCauseInput').val('');
//  RiskCauseValidation.text('');
//    } else {
//    RiskCauseValidation.text('Please enter a valid event.');
//    }
//  });

//  function updateRiskCauseAccordionContent() {
//    $('#RiskCausetablecontent').empty();
//    RiskCauseArray.forEach((event, index) => {
//      var html = '';
//  html += '<tr>';
//    html += '<td class="typtxt font-weight-bold" name="typtxt" id="TypeVal">' + (index + 1) + '</td>';
//    html += '<td class="filenames" data-fileid="' + index + '">' + event + '</td>';
//    html += '<td><a class="btn btn-danger delete-btn" data-toggle="tooltip" data-placement="top" title="Delete"><i class="bx bx-message-square-x"></i></a></td>';
//    html += '</tr>';
//  $('#RiskCausetablecontent').append(html);
//    });

//    updateHiddenRiskCause();
//  }
//  $('#RiskCausetablecontent').on('click', '.delete-btn', function () {
//    var index = $(this).closest('tr').index();
//  RiskCauseArray.splice(index, 1);
//  updateRiskCauseAccordionContent();
//  });

//    function updateHiddenRiskCause() {
//      $('#RiskCauseArrayInput').val(JSON.stringify(RiskCauseArray));
//    }

//  let RiskConsequenceArray = [];
//  $('#addRiskConsequence').click(function () {
//    const RiskConsequence = $('#RiskConsequenceInput').val().trim();
//  const RiskConsequenceValidation = $('#RiskConsequenceValidation');

//  if (RiskConsequence) {
//    RiskConsequenceArray.push(RiskConsequence);
//  updateRiskConsequenceAccordionContent();
//  $('#RiskConsequenceInput').val('');
//  RiskConsequenceValidation.text('');
//    } else {
//    RiskConsequenceValidation.text('Please enter a valid event.');
//    }
//  });

//  function updateRiskConsequenceAccordionContent() {
//    $('#RiskConsequencetablecontent').empty();
//    RiskConsequenceArray.forEach((event, index) => {
//      var html = '';
//  html += '<tr>';
//    html += '<td class="typtxt font-weight-bold" name="typtxt" id="TypeVal">' + (index + 1) + '</td>';
//    html += '<td class="filenames" data-fileid="' + index + '">' + event + '</td>';
//    html += '<td><a class="btn btn-danger delete-btn" data-toggle="tooltip" data-placement="top" title="Delete"><i class="bx bx-message-square-x"></i></a></td>';
//    html += '</tr>';
//  $('#RiskConsequencetablecontent').append(html);
//    });
//    updateHiddenRiskConsequence();
//  }
//  $('#RiskConsequencetablecontent').on('click', '.delete-btn', function () {
//    var index = $(this).closest('tr').index();
//  RiskConsequenceArray.splice(index, 1);
//  updateRiskConsequenceAccordionContent();
//  });

//    function updateHiddenRiskConsequence() {
//      $('#RiskConsequenceArrayInput').val(JSON.stringify(RiskConsequenceArray));
//    }

    
//});
