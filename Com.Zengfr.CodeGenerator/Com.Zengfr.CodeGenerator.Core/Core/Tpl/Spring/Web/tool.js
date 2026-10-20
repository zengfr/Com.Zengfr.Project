//http://stackoverflow.com/questions/9807426/use-jquery-to-re-populate-form-with-json-data
// reset form values from json object
$.each(data, function(name, val){
    var $el = $('[name="'+name+'"]'),
        type = $el.attr('type');

    switch(type){
        case 'checkbox':
            $el.attr('checked', 'checked');
            break;
        case 'radio':
            $el.filter('[value="'+val+'"]').attr('checked', 'checked');
            break;
        default:
            $el.val(val);
    }
});


var myData = $("form#form_id").serialize();
$.ajax({
    url: "my_save_script.php",
    type: "post",
    data: myData,
    success: function(msg){
        alert(msg);
    }
});

$('#mybutton').click(function() {
    $.post('/api/user', JSON.stringify($('#myform').jsonify()));
}
