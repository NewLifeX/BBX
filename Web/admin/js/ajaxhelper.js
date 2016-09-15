
//-----------------------------------------------compat----------------------------------------------

if (!Array.prototype.push) {
  Array.prototype.push = function() {
		var startLength = this.length;
		for (var i = 0; i < arguments.length; i++)
      this[startLength + i] = arguments[i];
	  return this.length;
  }
}

if (!Function.prototype.apply) {
  // Based on code from http://www.youngpup.net/
  Function.prototype.apply = function(object, parameters) {
    var parameterStrings = new Array();
    if (!object)     object = window;
    if (!parameters) parameters = new Array();
    
    for (var i = 0; i < parameters.length; i++)
      parameterStrings[i] = 'x[' + i + ']';
    
    object.__apply__ = this;
    var result = eval('obj.__apply__(' + 
      parameterStrings[i].join(', ') + ')');
    object.__apply__ = null;
    
    return result;
  }
}

//-----------------------------------------------prototype----------------------------------------------

var Prototype = {
  Version: '@@VERSION@@'
}

var Class = {
  create: function() {
    return function() { 
      this.initialize.apply(this, arguments);
    }
  }
}

var Abstract = new Object();

Object.prototype.extend = function(object) {
  for (property in object) {
    this[property] = object[property];
  }
  return this;
}

Function.prototype.bind = function(object) {
  var method = this;
  return function() {
    method.apply(object, arguments);
  }
}

Function.prototype.bindAsEventListener = function(object) {
  var method = this;
  return function(event) {
    method.call(object, event || window.event);
  }
}

Number.prototype.toColorPart = function() {
  var digits = this.toString(16);
  if (this < 16) return '0' + digits;
  return digits;
}

var Try = {
  these: function() {
    var returnValue;
    
    for (var i = 0; i < arguments.length; i++) {
      var lambda = arguments[i];
      try {
        returnValue = lambda();
        break;
      } catch (e) {}
    }
    
    return returnValue;
  }
}

/*--------------------------------------------------------------------------*/

var PeriodicalExecuter = Class.create();
PeriodicalExecuter.prototype = {
  initialize: function(callback, frequency) {
    this.callback = callback;
    this.frequency = frequency;
    this.currentlyExecuting = false;
    
    this.registerCallback();
  },
  
  registerCallback: function() {
    setTimeout(this.onTimerEvent.bind(this), this.frequency * 1000);
  },
  
  onTimerEvent: function() {
    if (!this.currentlyExecuting) {
      try { 
        this.currentlyExecuting = true;
        this.callback(); 
      } finally { 
        this.currentlyExecuting = false;
      }
    }
    
    this.registerCallback();
  }
}

/*--------------------------------------------------------------------------*/

function $() {
  var elements = new Array();
  
  for (var i = 0; i < arguments.length; i++) {
    var element = arguments[i];
    try
    {
		if (typeof element == 'string')
		element = document.getElementById(element) || document.all(element) || document.forms(0).all(element);
    }
    catch(ex)
    {
		element = null;
    }

    if (arguments.length == 1) 
      return element;
      
    elements.push(element);
  }
  
  return elements;
}

//---------------------------------------------------ajax-----------------------------------------------------

var Ajax = {
  getTransport: function() {
    return Try.these(
      function() {return new ActiveXObject('Msxml2.XMLHTTP')},
      function() {return new ActiveXObject('Microsoft.XMLHTTP')},
      function() {return new XMLHttpRequest()}
    ) || false;
  },
  
  emptyFunction: function() {}
}

Ajax.Base = function() {};
Ajax.Base.prototype = {
  setOptions: function(options) {
    this.options = {
      method:       'post',
      asynchronous: true,
      parameters:   ''
    }.extend(options || {});
  }
}

Ajax.Request = Class.create();
Ajax.Request.Events = 
  ['Uninitialized', 'Loading', 'Loaded', 'Interactive', 'Complete'];

Ajax.Request.prototype = (new Ajax.Base()).extend({
  initialize: function(url, options) {
    this.transport = Ajax.getTransport();
    this.setOptions(options);
  
    try {
      if (this.options.method == 'get')
        url += '?' + this.options.parameters + '&_=';
    
      this.transport.open(this.options.method, url, true);
      
      if (this.options.asynchronous) {
        this.transport.onreadystatechange = this.onStateChange.bind(this);
        setTimeout((function() {this.respondToReadyState(1)}).bind(this), 10);
      }
              
      this.transport.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
      this.transport.setRequestHeader('X-Prototype-Version', Prototype.Version);

      if (this.options.method == 'post') {
        this.transport.setRequestHeader('Connection', 'close');
        this.transport.setRequestHeader('Content-type',
          'application/x-www-form-urlencoded');
      }
      
      this.transport.send(this.options.method == 'post' ? 
        this.options.parameters + '&_=' : null);
                      
    } catch (e) {
    }    
  },
      
  onStateChange: function() {
    var readyState = this.transport.readyState;
    if (readyState != 1)
      this.respondToReadyState(this.transport.readyState);
  },
  
  respondToReadyState: function(readyState) {
    var event = Ajax.Request.Events[readyState];
    (this.options['on' + event] || Ajax.emptyFunction)(this.transport);
  }
});

Ajax.Updater = Class.create();
Ajax.Updater.prototype = (new Ajax.Base()).extend({
  initialize: function(container, url, options) {
    this.container = $(container);
    this.setOptions(options);
  
    if (this.options.asynchronous) {
      this.onComplete = this.options.onComplete;
      this.options.onComplete = this.updateContent.bind(this);
    }
    
    this.request = new Ajax.Request(url, this.options);
    
    if (!this.options.asynchronous)
      this.updateContent();
  },
  
  updateContent: function() {
    if (this.options.insertion) {
      new this.options.insertion(this.container,
        this.request.transport.responseText);
    } else {
      this.container.innerHTML = this.request.transport.responseText;
    }

    if (this.onComplete) {
      setTimeout((function() {this.onComplete(this.request)}).bind(this), 10);
    }
  }
});

//---------------------------------------------------form-----------------------------------------------------

var Field = {
  clear: function() {
    for (var i = 0; i < arguments.length; i++)
      $(arguments[i]).value = '';
  },

  focus: function(element) {
    $(element).focus();
  },
  
  present: function() {
    for (var i = 0; i < arguments.length; i++)
      if ($(arguments[i]).value == '') return false;
    return true;
  },
  
  select: function(element) {
    $(element).select();
  },
   
  activate: function(element) {
    $(element).focus();
    $(element).select();
  }
}

/*--------------------------------------------------------------------------*/

var Form = {
  serialize: function(form) {
    var elements = Form.getElements($(form));
    var queryComponents = new Array();
    
    for (var i = 0; i < elements.length; i++) {
      var queryComponent = Form.Element.serialize(elements[i]);
      if (queryComponent)
        queryComponents.push(queryComponent);
    }
    
    return queryComponents.join('&');
  },
  
  getElements: function(form) {
	form = $(form);
    var elements = new Array();

    for (tagName in Form.Element.Serializers) {
      var tagElements = form.getElementsByTagName(tagName);
      for (var j = 0; j < tagElements.length; j++)
        elements.push(tagElements[j]);
    }
    return elements;
  },
  
  disable: function(form) {
    var elements = Form.getElements(form);
    for (var i = 0; i < elements.length; i++) {
      var element = elements[i];
      element.blur();
      element.disable = 'true';
    }
  },

  focusFirstElement: function(form) {
    form = $(form);
    var elements = Form.getElements(form);
    for (var i = 0; i < elements.length; i++) {
      var element = elements[i];
      if (element.type != 'hidden' && !element.disabled) {
        Field.activate(element);
        break;
      }
    }
  },

  reset: function(form) {
    $(form).reset();
  }
}

Form.Element = {
  serialize: function(element) {
    element = $(element);
    var method = element.tagName.toLowerCase();
    var parameter = Form.Element.Serializers[method](element);
    
    if (parameter)
      return encodeURIComponent(parameter[0]) + '=' + 
        encodeURIComponent(parameter[1]);                   
  },
  
  getValue: function(element) {
    element = $(element);
    var method = element.tagName.toLowerCase();
    var parameter = Form.Element.Serializers[method](element);
    
    if (parameter) 
      return parameter[1];
  }
}

Form.Element.Serializers = {
  input: function(element) {
    switch (element.type.toLowerCase()) {
      case 'hidden':
      case 'password':
      case 'text':
        return Form.Element.Serializers.textarea(element);
      case 'checkbox':  
      case 'radio':
        return Form.Element.Serializers.inputSelector(element);
    }
    return false;
  },

  inputSelector: function(element) {
    if (element.checked)
      return [element.name, element.value];
  },

  textarea: function(element) {
    return [element.name, element.value];
  },

  select: function(element) {
    var index = element.selectedIndex;
    var value = element.options[index].value || element.options[index].text;
    return [element.name, (index >= 0) ? value : ''];
  }
}

/*--------------------------------------------------------------------------*/

var $F = Form.Element.getValue;

/*--------------------------------------------------------------------------*/

Abstract.TimedObserver = function() {}
Abstract.TimedObserver.prototype = {
  initialize: function(element, frequency, callback) {
    this.frequency = frequency;
    this.element   = $(element);
    this.callback  = callback;
    
    this.lastValue = this.getValue();
    this.registerCallback();
  },
  
  registerCallback: function() {
    setTimeout(this.onTimerEvent.bind(this), this.frequency * 1000);
  },
  
  onTimerEvent: function() {
    var value = this.getValue();
    if (this.lastValue != value) {
      this.callback(this.element, value);
      this.lastValue = value;
    }
    
    this.registerCallback();
  }
}

Form.Element.Observer = Class.create();
Form.Element.Observer.prototype = (new Abstract.TimedObserver()).extend({
  getValue: function() {
    return Form.Element.getValue(this.element);
  }
});

Form.Observer = Class.create();
Form.Observer.prototype = (new Abstract.TimedObserver()).extend({
  getValue: function() {
    return Form.serialize(this.element);
  }
});

//---------------------------------------------------AjaxHelper------------------------------------------------

var AjaxProxyUrl = new String("../ajax.aspx");

var AjaxHelper = 
{
    
	Updater : function(ajaxTemplate, output, params, onComplete)
	{	
		if (typeof output == 'string')
		{
			output = $(output);
		}
	
		var FormatContent = function(str)
		{
			var content = new String(str);
			var prefix = new String("<!--AjaxContent-->");
			content = content.substring(content.indexOf(prefix, 0) + prefix.length, content.length - 9);
			return content;
		}
		
		this.result="1";
		new Ajax.Request( AjaxProxyUrl,
		{ onComplete: function(transport) 
		  { 
		    if (output != null)
		     { output.innerHTML = FormatContent(transport.responseText); }
		    if (onComplete != null) 
		     { onComplete(FormatContent(transport.responseText)) } 
	       }, parameters: params + '&AjaxTemplate=' + ajaxTemplate 
	    });
	    
	    return this.result;
	},

  
	SerializeForm : function(form)
	{
		return Form.serialize(form);
	}
	
}

