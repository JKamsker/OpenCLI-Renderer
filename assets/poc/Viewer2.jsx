import React, { useState, useRef, useEffect } from 'react';
import { 
  UploadCloud, ChevronRight, ChevronDown, Terminal, 
  BookOpen, Code, FileJson, AlertCircle, Search, 
  ShieldAlert, Settings2, Info, ArrowRight, CornerDownRight,
  PanelRightOpen, PanelRightClose, Copy, CheckCircle2,
  TerminalSquare
} from 'lucide-react';

// --- Parsers ---

// Parses the OpenCLI JSON format
const parseJSON = (jsonString) => {
  try {
    const data = JSON.parse(jsonString);
    if (!data.commands) throw new Error("Invalid OpenCLI JSON: Missing 'commands' array.");
    return {
      info: data.info || { title: "CLI Document", version: "unknown" },
      commands: data.commands
    };
  } catch (err) {
    throw new Error(`JSON Parsing Error: ${err.message}`);
  }
};

// Parses the provided XML Document format
const parseXML = (xmlString) => {
  try {
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(xmlString, "text/xml");
    
    const errorNode = xmlDoc.querySelector("parsererror");
    if (errorNode) throw new Error("Invalid XML structure");

    const modelNode = xmlDoc.querySelector("Model");
    if (!modelNode) throw new Error("Missing <Model> root element");

    const parseXMLCommand = (node) => {
      const cmd = {
        name: node.getAttribute("Name") || "unnamed",
        description: "",
        commands: [],
        options: [],
        arguments: []
      };

      for (let child of node.children) {
        if (child.nodeName === 'Description') {
          cmd.description = child.textContent;
        } else if (child.nodeName === 'Command') {
          cmd.commands.push(parseXMLCommand(child));
        } else if (child.nodeName === 'Parameters') {
          for (let param of child.children) {
            if (param.nodeName === 'Option') {
              const short = param.getAttribute('Short');
              const long = param.getAttribute('Long');
              
              const aliases = [];
              if (short) aliases.push(`-${short}`);
              
              const optionName = long ? `--${long}` : (short ? `-${short}` : '--unknown');
              
              const opt = {
                name: optionName,
                aliases: aliases,
                description: "",
                required: param.getAttribute('Required') === 'true',
                arguments: []
              };

              const value = param.getAttribute('Value');
              if (value && value !== 'NULL') {
                 opt.arguments.push({
                   name: value,
                   required: true,
                   metadata: [{ name: 'ClrType', value: param.getAttribute('ClrType') || 'Unknown' }]
                 });
              }

              const descNode = Array.from(param.children).find(n => n.nodeName === 'Description');
              if (descNode) opt.description = descNode.textContent;

              cmd.options.push(opt);
            }
          }
        }
      }
      return cmd;
    };

    const commands = Array.from(modelNode.children)
      .filter(n => n.nodeName === 'Command')
      .map(parseXMLCommand);

    return {
      info: { title: "XML CLI Model", version: "1.0" },
      commands: commands
    };
  } catch (err) {
    throw new Error(`XML Parsing Error: ${err.message}`);
  }
};


// --- Components ---

const Badge = ({ children, variant = 'default', className = '' }) => {
  const variants = {
    default: "bg-slate-100 text-slate-700 border-slate-200 dark:bg-slate-800 dark:text-slate-300 dark:border-slate-700",
    primary: "bg-blue-50 text-blue-700 border-blue-200 dark:bg-blue-900/30 dark:text-blue-300 dark:border-blue-800",
    danger: "bg-red-50 text-red-700 border-red-200 dark:bg-red-900/30 dark:text-red-300 dark:border-red-800",
    success: "bg-green-50 text-green-700 border-green-200 dark:bg-green-900/30 dark:text-green-300 dark:border-green-800"
  };
  return (
    <span className={`px-2 py-0.5 text-xs font-medium border rounded-full ${variants[variant]} ${className}`}>
      {children}
    </span>
  );
};

const TypeFormatter = ({ clrType }) => {
  if (!clrType) return null;
  // Simplify long C# types
  let display = clrType;
  if (display.includes('System.String')) display = 'String';
  if (display.includes('System.Boolean')) display = 'Boolean';
  if (display.includes('System.Int32')) display = 'Int32';
  if (display.includes('Nullable')) display += '?';
  
  return <span className="font-mono text-xs text-amber-600 dark:text-amber-400">{display}</span>;
};

const TreeNode = ({ command, path, selectedPath, onSelect, searchQuery }) => {
  const [expanded, setExpanded] = useState(true);
  const isSelected = JSON.stringify(path) === JSON.stringify(selectedPath);
  const hasChildren = command.commands && command.commands.length > 0;
  
  // Basic search filtering (expands and shows if children match)
  const matchesSearch = command.name.toLowerCase().includes(searchQuery.toLowerCase());
  const hasMatchingDescendant = (cmd) => {
    if (cmd.name.toLowerCase().includes(searchQuery.toLowerCase())) return true;
    if (cmd.commands) {
      return cmd.commands.some(c => hasMatchingDescendant(c));
    }
    return false;
  };
  const shouldShow = searchQuery === '' || hasMatchingDescendant(command);

  if (!shouldShow) return null;

  return (
    <div className="ml-2">
      <div 
        className={`flex items-center py-1.5 px-2 rounded-md cursor-pointer select-none group transition-colors ${
          isSelected 
            ? 'bg-blue-100 text-blue-900 dark:bg-blue-900/40 dark:text-blue-100' 
            : 'text-slate-700 dark:text-slate-300 hover:bg-slate-100 dark:hover:bg-slate-800/50'
        }`}
        onClick={(e) => {
          e.stopPropagation();
          onSelect(path, command);
          if (hasChildren && !expanded) setExpanded(true);
        }}
      >
        <span 
          className="w-5 h-5 flex items-center justify-center mr-1"
          onClick={(e) => {
            if (hasChildren) {
              e.stopPropagation();
              setExpanded(!expanded);
            }
          }}
        >
          {hasChildren ? (
            expanded ? <ChevronDown size={14} className="opacity-70" /> : <ChevronRight size={14} className="opacity-70" />
          ) : (
            <Terminal size={12} className="opacity-40" />
          )}
        </span>
        <span className="text-sm font-medium font-mono">{command.name}</span>
      </div>
      
      {hasChildren && expanded && (
        <div className="ml-3 border-l border-slate-200 dark:border-slate-800 pl-1 mt-1">
          {command.commands.map((child, idx) => (
            <TreeNode 
              key={idx}
              command={child}
              path={[...path, child.name]}
              selectedPath={selectedPath}
              onSelect={onSelect}
              searchQuery={searchQuery}
            />
          ))}
        </div>
      )}
    </div>
  );
};


// --- Main Application ---

export default function App() {
  const [cliData, setCliData] = useState(null);
  const [error, setError] = useState('');
  const [selectedPath, setSelectedPath] = useState([]);
  const [selectedCommand, setSelectedCommand] = useState(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [isDragging, setIsDragging] = useState(false);
  const [isComposerOpen, setIsComposerOpen] = useState(false);
  const [composerValues, setComposerValues] = useState({});
  const [copied, setCopied] = useState(false);
  const fileInputRef = useRef(null);

  // Reset composer values when selected command changes
  useEffect(() => {
    setComposerValues({});
    setCopied(false);
  }, [selectedCommand]);

  const handleFileProcess = (file) => {
    setError('');
    const reader = new FileReader();
    reader.onload = (e) => {
      const content = e.target.result;
      try {
        let parsed;
        if (file.name.endsWith('.xml')) {
          parsed = parseXML(content);
        } else {
          // Default to JSON
          parsed = parseJSON(content);
        }
        setCliData(parsed);
        // Select first command by default
        if (parsed.commands && parsed.commands.length > 0) {
          setSelectedPath([parsed.commands[0].name]);
          setSelectedCommand(parsed.commands[0]);
        }
      } catch (err) {
        setError(err.message);
      }
    };
    reader.onerror = () => setError("Failed to read file");
    reader.readAsText(file);
  };

  const onDragOver = (e) => {
    e.preventDefault();
    setIsDragging(true);
  };

  const onDragLeave = () => setIsDragging(false);

  const onDrop = (e) => {
    e.preventDefault();
    setIsDragging(false);
    if (e.dataTransfer.files && e.dataTransfer.files.length > 0) {
      handleFileProcess(e.dataTransfer.files[0]);
    }
  };

  const generateCommandString = () => {
    if (!selectedCommand || !cliData) return '';
    const rootName = cliData.info?.title || 'cli';
    
    // Command base (e.g. "jdr accounts add")
    const parts = [rootName, ...selectedPath];

    // Append options based on composer state
    if (selectedCommand.options) {
      selectedCommand.options.forEach(opt => {
        const val = composerValues[opt.name];
        if (val !== undefined && val !== false && val !== '') {
          const isFlag = !opt.arguments || opt.arguments.length === 0;
          if (isFlag) {
            parts.push(opt.name); // e.g., --verbose
          } else {
            // Check if string contains spaces to add quotes
            const stringVal = String(val);
            const safeVal = stringVal.includes(' ') ? `"${stringVal}"` : stringVal;
            parts.push(`${opt.name} ${safeVal}`);
          }
        }
      });
    }

    return parts.join(' ');
  };

  const handleCopyCommand = () => {
    const cmdString = generateCommandString();
    try {
      if (navigator.clipboard && navigator.clipboard.writeText) {
        navigator.clipboard.writeText(cmdString);
        setCopied(true);
      } else {
        // Fallback for some iframes
        const el = document.createElement('textarea');
        el.value = cmdString;
        document.body.appendChild(el);
        el.select();
        document.execCommand('copy');
        document.body.removeChild(el);
        setCopied(true);
      }
    } catch (err) {
      console.error('Failed to copy text', err);
    }
    setTimeout(() => setCopied(false), 2000);
  };

  const handleComposerChange = (optName, value) => {
    setComposerValues(prev => ({ ...prev, [optName]: value }));
  };


  // View: File Uploader 
  if (!cliData) {
    return (
      <div 
        className={`min-h-screen flex items-center justify-center p-6 transition-colors duration-200 ${
          isDragging ? 'bg-blue-50 dark:bg-slate-900' : 'bg-slate-50 dark:bg-slate-950'
        }`}
        onDragOver={onDragOver}
        onDragLeave={onDragLeave}
        onDrop={onDrop}
      >
        <div className={`max-w-xl w-full p-10 rounded-2xl border-2 border-dashed flex flex-col items-center text-center space-y-6 bg-white dark:bg-slate-900 shadow-xl ${
          isDragging ? 'border-blue-500 shadow-blue-500/20' : 'border-slate-300 dark:border-slate-700'
        }`}>
          <div className="w-20 h-20 bg-blue-100 dark:bg-blue-900/50 rounded-full flex items-center justify-center text-blue-600 dark:text-blue-400">
            <Code size={40} />
          </div>
          <div>
            <h1 className="text-2xl font-bold text-slate-800 dark:text-white mb-2">OpenCLI Viewer</h1>
            <p className="text-slate-500 dark:text-slate-400">
              Drag and drop an <code className="bg-slate-100 dark:bg-slate-800 px-1 py-0.5 rounded text-sm">opencli.json</code> or XML spec file here.
            </p>
          </div>
          
          {error && (
            <div className="w-full p-4 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg flex items-start text-left text-red-600 dark:text-red-400">
              <AlertCircle className="shrink-0 mr-3 mt-0.5" size={18} />
              <span className="text-sm">{error}</span>
            </div>
          )}

          <button 
            onClick={() => fileInputRef.current?.click()}
            className="px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white rounded-lg font-medium transition-colors shadow-lg shadow-blue-600/20 flex items-center"
          >
            <UploadCloud className="mr-2" size={20} />
            Select File
          </button>
          <input 
            type="file" 
            ref={fileInputRef} 
            onChange={(e) => e.target.files?.[0] && handleFileProcess(e.target.files[0])} 
            className="hidden" 
            accept=".json,.xml"
          />
        </div>
      </div>
    );
  }

  // View: Main Dashboard
  return (
    <div className="flex flex-col h-screen bg-white dark:bg-slate-950 text-slate-800 dark:text-slate-200">
      {/* Top Navbar */}
      <header className="h-14 border-b border-slate-200 dark:border-slate-800 bg-slate-50 dark:bg-slate-900 flex items-center justify-between px-4 shrink-0">
        <div className="flex items-center space-x-3">
          <div className="bg-blue-600 text-white p-1.5 rounded-md">
            <Terminal size={18} />
          </div>
          <h1 className="font-semibold text-sm">
            {cliData.info?.title} <span className="opacity-50 ml-2 font-normal">v{cliData.info?.version}</span>
          </h1>
        </div>
        <div className="flex items-center space-x-2">
          <button 
            onClick={() => setIsComposerOpen(!isComposerOpen)}
            className={`flex items-center px-3 py-1.5 rounded-md text-sm font-medium transition-colors border ${
              isComposerOpen 
                ? 'bg-blue-50 text-blue-700 border-blue-200 dark:bg-blue-900/30 dark:text-blue-300 dark:border-blue-800' 
                : 'bg-white text-slate-700 border-slate-200 hover:bg-slate-50 dark:bg-slate-950 dark:text-slate-300 dark:border-slate-800 dark:hover:bg-slate-900'
            }`}
          >
            {isComposerOpen ? <PanelRightClose size={16} className="mr-2" /> : <PanelRightOpen size={16} className="mr-2" />}
            Composer
          </button>
          <div className="w-px h-6 bg-slate-300 dark:bg-slate-700 mx-1"></div>
          <button 
            onClick={() => setCliData(null)}
            className="text-xs font-medium px-3 py-1.5 rounded-md text-slate-600 hover:bg-slate-200 dark:text-slate-400 dark:hover:bg-slate-800 transition-colors"
          >
            Close File
          </button>
        </div>
      </header>

      {/* Main Layout */}
      <div className="flex flex-1 overflow-hidden">
        
        {/* Sidebar Navigation */}
        <aside className="w-72 flex flex-col border-r border-slate-200 dark:border-slate-800 bg-slate-50 dark:bg-slate-900/50 shrink-0">
          <div className="p-4 border-b border-slate-200 dark:border-slate-800">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" size={16} />
              <input 
                type="text" 
                placeholder="Filter commands..." 
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="w-full pl-9 pr-3 py-2 bg-white dark:bg-slate-950 border border-slate-200 dark:border-slate-800 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition-shadow"
              />
            </div>
          </div>
          <div className="flex-1 overflow-y-auto p-2">
            {cliData.commands.map((cmd, idx) => (
              <TreeNode 
                key={idx} 
                command={cmd} 
                path={[cmd.name]} 
                selectedPath={selectedPath} 
                onSelect={(p, c) => { setSelectedPath(p); setSelectedCommand(c); }}
                searchQuery={searchQuery}
              />
            ))}
          </div>
        </aside>

        {/* Content Area */}
        <main className="flex-1 overflow-y-auto bg-white dark:bg-slate-950 p-8">
          {selectedCommand ? (
            <div className="max-w-4xl mx-auto pb-20">
              
              {/* Breadcrumb Header */}
              <div className="flex items-center text-sm text-slate-500 dark:text-slate-400 mb-6 font-mono bg-slate-50 dark:bg-slate-900 w-max px-3 py-1 rounded-md border border-slate-200 dark:border-slate-800">
                <span className="text-slate-800 dark:text-slate-200">{cliData.info?.title}</span>
                {selectedPath.map((p, i) => (
                  <React.Fragment key={i}>
                    <ChevronRight size={14} className="mx-1" />
                    <span className={i === selectedPath.length - 1 ? 'text-blue-600 dark:text-blue-400 font-semibold' : ''}>
                      {p}
                    </span>
                  </React.Fragment>
                ))}
              </div>

              {/* Title & Description */}
              <h2 className="text-3xl font-bold font-mono text-slate-900 dark:text-white mb-4">
                {selectedCommand.name}
              </h2>
              {selectedCommand.description ? (
                <p className="text-lg text-slate-600 dark:text-slate-300 mb-8 leading-relaxed">
                  {selectedCommand.description}
                </p>
              ) : (
                <p className="text-lg text-slate-400 italic mb-8">No description provided.</p>
              )}

              {/* Subcommands Overview */}
              {selectedCommand.commands && selectedCommand.commands.length > 0 && (
                <section className="mb-10">
                  <h3 className="text-lg font-semibold flex items-center mb-4 pb-2 border-b border-slate-200 dark:border-slate-800">
                    <BookOpen size={18} className="mr-2 text-blue-500" />
                    Subcommands
                  </h3>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                    {selectedCommand.commands.map((sub, i) => (
                      <div 
                        key={i} 
                        onClick={() => {
                          setSelectedPath([...selectedPath, sub.name]);
                          setSelectedCommand(sub);
                        }}
                        className="p-4 border border-slate-200 dark:border-slate-800 rounded-lg hover:border-blue-400 dark:hover:border-blue-500 hover:bg-slate-50 dark:hover:bg-slate-900 cursor-pointer transition-all group"
                      >
                        <div className="flex items-center justify-between mb-1">
                          <h4 className="font-mono font-medium text-blue-600 dark:text-blue-400">
                            {sub.name}
                          </h4>
                          <ArrowRight size={16} className="text-slate-300 group-hover:text-blue-500 transition-colors" />
                        </div>
                        <p className="text-sm text-slate-500 dark:text-slate-400 line-clamp-2">
                          {sub.description || "No description"}
                        </p>
                      </div>
                    ))}
                  </div>
                </section>
              )}

              {/* Command Options */}
              {selectedCommand.options && selectedCommand.options.length > 0 && (
                <section className="mb-10">
                  <h3 className="text-lg font-semibold flex items-center mb-4 pb-2 border-b border-slate-200 dark:border-slate-800">
                    <Settings2 size={18} className="mr-2 text-indigo-500" />
                    Options
                  </h3>
                  <div className="space-y-4">
                    {selectedCommand.options.map((opt, i) => (
                      <div key={i} className="bg-slate-50 dark:bg-slate-900 rounded-lg border border-slate-200 dark:border-slate-800 overflow-hidden">
                        <div className="px-4 py-3 bg-slate-100/50 dark:bg-slate-800/50 border-b border-slate-200 dark:border-slate-800 flex flex-wrap items-center gap-3">
                          <span className="font-mono font-bold text-slate-800 dark:text-slate-200">
                            {opt.name}
                          </span>
                          
                          {opt.aliases && opt.aliases.map((alias, aIdx) => (
                            <span key={aIdx} className="font-mono text-slate-500">
                              {alias}
                            </span>
                          ))}

                          {opt.required && <Badge variant="danger">Required</Badge>}
                          
                          {opt.arguments && opt.arguments.map((arg, aIdx) => {
                            const clrType = arg.metadata?.find(m => m.name === 'ClrType')?.value;
                            return (
                              <div key={aIdx} className="flex items-center space-x-2 ml-auto">
                                <span className="text-xs font-mono text-slate-400 uppercase tracking-wider">{arg.name}</span>
                                {clrType && <TypeFormatter clrType={clrType} />}
                              </div>
                            );
                          })}
                        </div>
                        <div className="px-4 py-3 text-sm text-slate-600 dark:text-slate-300">
                          {opt.description || "No description available."}
                        </div>
                      </div>
                    ))}
                  </div>
                </section>
              )}

              {/* Example Section */}
              {selectedCommand.examples && selectedCommand.examples.length > 0 && (
                <section className="mb-10">
                  <h3 className="text-lg font-semibold flex items-center mb-4 pb-2 border-b border-slate-200 dark:border-slate-800">
                    <Info size={18} className="mr-2 text-green-500" />
                    Examples
                  </h3>
                  <div className="space-y-3">
                    {selectedCommand.examples.map((ex, i) => (
                      <div key={i} className="bg-slate-900 rounded-lg p-4 font-mono text-sm text-green-400">
                        $ {ex}
                      </div>
                    ))}
                  </div>
                </section>
              )}

              {/* Empty state when leaf node has no options */}
              {(!selectedCommand.commands?.length && !selectedCommand.options?.length && !selectedCommand.examples?.length) && (
                <div className="text-center py-12 text-slate-500 border-2 border-dashed border-slate-200 dark:border-slate-800 rounded-xl">
                  <Terminal size={40} className="mx-auto mb-3 opacity-20" />
                  <p>No additional details or options defined for this command.</p>
                </div>
              )}

            </div>
          ) : (
            <div className="h-full flex items-center justify-center text-slate-400">
              <div className="text-center">
                <CornerDownRight size={48} className="mx-auto mb-4 opacity-20" />
                <p>Select a command from the sidebar to view details.</p>
              </div>
            </div>
          )}
        </main>

        {/* Right Sidebar: Command Composer */}
        {isComposerOpen && (
          <aside className="w-80 flex flex-col border-l border-slate-200 dark:border-slate-800 bg-white dark:bg-slate-950 shrink-0 shadow-[-4px_0_15px_-3px_rgba(0,0,0,0.05)] dark:shadow-none transition-all duration-300 z-10">
            <div className="p-4 border-b border-slate-200 dark:border-slate-800 bg-slate-50 dark:bg-slate-900/50 flex items-center justify-between">
              <h3 className="font-semibold text-slate-800 dark:text-slate-200 flex items-center">
                <TerminalSquare size={18} className="mr-2 text-blue-500" />
                Composer
              </h3>
            </div>
            
            <div className="flex-1 overflow-y-auto p-4 space-y-6">
              {!selectedCommand ? (
                 <div className="text-center text-sm text-slate-400 mt-10">Select a command to start composing.</div>
              ) : selectedCommand.options && selectedCommand.options.length > 0 ? (
                <div className="space-y-4">
                  <p className="text-sm font-medium text-slate-500 dark:text-slate-400 border-b border-slate-200 dark:border-slate-800 pb-2">
                    Options for <span className="font-mono text-slate-800 dark:text-slate-200">{selectedCommand.name}</span>
                  </p>
                  {selectedCommand.options.map((opt, idx) => {
                    const isFlag = !opt.arguments || opt.arguments.length === 0;
                    
                    if (isFlag) {
                      return (
                        <label key={idx} className="flex items-start space-x-3 cursor-pointer group">
                          <input 
                            type="checkbox" 
                            checked={!!composerValues[opt.name]}
                            onChange={(e) => handleComposerChange(opt.name, e.target.checked)}
                            className="mt-1 w-4 h-4 rounded border-slate-300 text-blue-600 focus:ring-blue-500 dark:border-slate-700 dark:bg-slate-900 cursor-pointer" 
                          />
                          <div className="flex-1">
                            <span className="font-mono text-sm text-slate-800 dark:text-slate-200 block group-hover:text-blue-600 dark:group-hover:text-blue-400 transition-colors">
                              {opt.name} {opt.required && <span className="text-red-500">*</span>}
                            </span>
                            {opt.description && (
                              <span className="text-xs text-slate-500 dark:text-slate-400 line-clamp-1 mt-0.5" title={opt.description}>
                                {opt.description}
                              </span>
                            )}
                          </div>
                        </label>
                      );
                    } else {
                      return (
                        <div key={idx} className="space-y-1.5">
                          <label className="font-mono text-sm text-slate-800 dark:text-slate-200 block">
                            {opt.name} {opt.required && <span className="text-red-500">*</span>}
                          </label>
                          <input 
                            type="text" 
                            placeholder={opt.arguments?.[0]?.name || "value"}
                            value={composerValues[opt.name] || ''}
                            onChange={(e) => handleComposerChange(opt.name, e.target.value)}
                            className="w-full px-3 py-1.5 bg-white dark:bg-slate-900 border border-slate-200 dark:border-slate-800 rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition-shadow text-slate-800 dark:text-slate-200 placeholder:text-slate-400 dark:placeholder:text-slate-600 font-mono"
                          />
                        </div>
                      );
                    }
                  })}
                </div>
              ) : (
                <div className="text-center text-sm text-slate-400 mt-10">This command takes no configurable options.</div>
              )}
            </div>

            {/* Generated Command Preview Box */}
            <div className="p-4 bg-slate-100 dark:bg-slate-900 border-t border-slate-200 dark:border-slate-800">
              <label className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-2 block">Generated Command</label>
              <div className="relative group">
                <div className="w-full min-h-[5rem] p-3 pr-12 bg-slate-950 dark:bg-black rounded-lg text-green-400 font-mono text-sm break-all">
                  {generateCommandString() || "..."}
                </div>
                <button 
                  onClick={handleCopyCommand}
                  disabled={!selectedCommand}
                  className="absolute top-2 right-2 p-1.5 text-slate-400 hover:text-white bg-slate-800 hover:bg-slate-700 rounded-md transition-all disabled:opacity-50 disabled:cursor-not-allowed"
                  title="Copy to clipboard"
                >
                  {copied ? <CheckCircle2 size={16} className="text-green-500" /> : <Copy size={16} />}
                </button>
              </div>
            </div>
          </aside>
        )}
      </div>
    </div>
  );
}